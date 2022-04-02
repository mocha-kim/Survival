using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attribute is character's physical ability or skill
 * This has modified value and base value
 * 
 * modified value : This can be modified by equipment or condition effects
 * base value : This can only be improved by character's experience
 */

[Serializable]
public class Attribute
{
    public AttributeType type;
    public int modifiedValue;
    public int baseValue;
    public float exp;

    public Attribute(AttributeType type, int baseValue)
    {
        this.type = type;
        this.baseValue = baseValue;
        modifiedValue = baseValue;
        exp = 0f;
    }
}
