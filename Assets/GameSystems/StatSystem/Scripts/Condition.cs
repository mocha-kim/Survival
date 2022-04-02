using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Condition
{
    public ConditionType type;
    public ConditionEffect[] effects;

    public Sprite icon;

    public bool isActive;
    public float activationTime;

    public int DisplayTime
    {
        get
        {
            if (activationTime >= 60)
            {
                return Mathf.FloorToInt(activationTime / 60);
            }
            return Mathf.FloorToInt(activationTime);
        }
    }

    public Condition(ConditionType type)
    {
        this.type = type;
        isActive = false;
        activationTime = 0f;
    }

    public string TypeToKorean()
    {
        switch (type)
        {
            case ConditionType.SwallowWound:
                return "얕은상처";
            case ConditionType.Deepwound:
                return "깊은상처";
            case ConditionType.Fracture:
                return "골절";
            case ConditionType.Drained:
                return "탈진";
            case ConditionType.Dehydration:
                return "탈수";
            case ConditionType.Starvation:
                return "굶주림";
            case ConditionType.Infection:
                return "감염";
            case ConditionType.FoodPoisoning:
                return "식중독";
            case ConditionType.MentalBreakdown:
                return "멘탈붕괴";
            default:
                return "";
        }
    }
}
