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
}
