using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IFlagManager
{
    Sprite random();
    Sprite GetNextFlag();
    bool HasMoreFlags();
    int GetFlagCount();
    void EnqueueFlags(int count);
}
