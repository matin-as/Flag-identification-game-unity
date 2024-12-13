using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using Random = System.Random;

public class QuestionManager : MonoBehaviour, IUIManager
{
    [SerializeField] private TextMeshProUGUI t_point;
    [SerializeField] private Image flagimage;
    [SerializeField] private GameObject parent_ans;
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject start_menu;
    [SerializeField] private GameObject end_menu;
    [SerializeField] private TMP_InputField number_qs;

    private IFlagManager flagManager;
    private int point = 0;

    void Start()
    {
        Sprite[] flags = Resources.LoadAll<Sprite>("images");
        flagManager = new FlagManager(flags);
    }

    public void On_ClickStart()
    {
        if (string.IsNullOrEmpty(number_qs.text))
        {
            return;
        }
        play.SetActive(true);
        start_menu.SetActive(false);
        int numberOfQuestions = int.Parse(number_qs.text);
        flagManager.EnqueueFlags(numberOfQuestions);
        ShowNextQuestion();
    }

    public void ShowNextQuestion()
    {
        print(flagManager.GetFlagCount());
        if (!flagManager.HasMoreFlags())
        {
            play.SetActive(false);
            end_menu.SetActive(true);
            return;
        }


        flagimage.sprite = flagManager.GetNextFlag();

        Random random = new Random();
        for (int i = 0; i < parent_ans.transform.childCount; i++)
        {
            parent_ans.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            Sprite randomIndex = flagManager.random();
            parent_ans.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = randomIndex.name;
            parent_ans.transform.GetChild(i).GetComponent<Button>().AddEventListener(randomIndex.name, parent_ans.transform.GetChild(i).GetComponent<Image>(), HandleClick);
        }
        int randomChildIndex = random.Next(0, parent_ans.transform.childCount);
        parent_ans.transform.GetChild(randomChildIndex).GetChild(0).GetComponent<TextMeshProUGUI>().text = flagimage.sprite.name;
        parent_ans.transform.GetChild(randomChildIndex).GetComponent<Button>().onClick.RemoveAllListeners();
        parent_ans.transform.GetChild(randomChildIndex).GetComponent<Button>().AddEventListener(flagimage.sprite.name, parent_ans.transform.GetChild(randomChildIndex).GetComponent<Image>(), HandleClick);
    }

    public void HandleClick(string flagName, Image image)
    {
        if (flagName == flagimage.sprite.name)
        {
            image.color = Color.green;
            point++;
            UpdatePoints(point);
        }
        else
        {
            image.color = Color.red;
        }
        StartCoroutine(ResetColor(image));
    }

    IEnumerator ResetColor(Image image)
    {
        yield return new WaitForSeconds(0.5f);
        image.color = Color.white;
        print("Called");
        ShowNextQuestion();
    }

    public void UpdatePoints(int points)
    {
        t_point.text = points.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}

public static class ButtonExtension
{
    public static void AddEventListener<T1, T2>(this Button button, T1 param1, T2 param2, Action<T1, T2> OnClick)
    {
        button.onClick.AddListener(delegate () {
            OnClick(param1, param2);
        });
    }
}
