using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;
public class FlagManager : IFlagManager
{
    private Queue<Sprite> usedFlags = new Queue<Sprite>();
    private Sprite[] allFlags;

    public FlagManager(Sprite[] flags)
    {
        allFlags = flags;
    }

    public void EnqueueFlags(int count)
    {
        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = random.Next(0, allFlags.Length);
            usedFlags.Enqueue(allFlags[randomIndex]);
        }
    }

    public Sprite GetNextFlag()
    {
        return usedFlags.Dequeue();
    }

    public bool HasMoreFlags()
    {
        return usedFlags.Count > 0;
    }

    public int GetFlagCount()
    {
        return usedFlags.Count;
    }

    public Sprite random()
    {
        Random rnd = new Random();
        return allFlags[rnd.Next(0, allFlags.Length)];
    }
}
