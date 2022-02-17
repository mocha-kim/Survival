using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemEffect
{
    public EffectType type;

    public StatusType statusType;
    public AttributeType attributeType;
    public ConditionType conditionType;

    public int value;
}
