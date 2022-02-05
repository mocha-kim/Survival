using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Status
{
    public StatusType type;
    public int currentValue;
    public int modifiedValue;
    public int baseValue;

    public float GetPercentage()
    {
        return (modifiedValue > 0 ? ((float)currentValue / (float)modifiedValue) : 0f);
    }
}