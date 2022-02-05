using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stat System/New Stats")]
public class StatsObject : ScriptableObject
{
    // Index order must be the same as order of each types
    // This makes possible to directly access by type(use type by index)
    public Attribute[] abilities;
    public Status[] statuses;
    public Condition[] conditions;

    public Action<StatsObject> OnStatChanged;
    private bool isInitialized = false;

    private void OnEnable()
    {
        InitStats();
    }

    private void InitStats()
    {
        if (isInitialized)
            return;

        isInitialized = true;

        // init abilities
        // init statuses
        // init conditions
    }

    // value change functions

    public void AddAttributeExp(AttributeType type, float value)
    {

    }
}
