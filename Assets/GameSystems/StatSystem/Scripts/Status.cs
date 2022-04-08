using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Status is directly related to Chracter's survival
 * This values always displayed on UI in percentage format
 * This has current value and maximun value
 * 
 * current value : This can be directly modified by food items, condition effets, enemy's attack...
 * max value : This can only be modifieded by equipment, attribute value. This value cannot be changed directly.
 */

[Serializable]
public class Status
{
    public StatusType type;
    public float currentValue;
    public float maxValue;

    public float Percentage
    {
        get
        {
            return (maxValue > 0 ? ((float)currentValue / (float)maxValue) : 0f);
        }
    }

    public Status(StatusType type, float maxValue)
    {
        this.type = type;
        this.maxValue = maxValue;
        currentValue = maxValue;
    }
}