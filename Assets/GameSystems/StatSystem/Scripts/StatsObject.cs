using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stat System/New Stats")]
public class StatsObject : ScriptableObject
{
    // Index order must be the same as order of each types
    // This makes possible to directly access by type(use type by index)
    public Status[] statuses;
    public Attribute[] attributes;
    public Condition[] conditions;

    public Action<StatsObject> OnStatChanged;
    public Action<StatsObject, Condition> OnConditionChanged;
    [NonSerialized]
    public bool isInitialized = false;

    public int CountActivatedConditions
    {
        get
        {
            int count = 0;
            foreach (Condition condition in conditions)
            {
                if (condition.isActive)
                {
                    count++;
                }
            }
            return count;
        }
    }

    private void OnEnable()
    {
        InitStats();
    }

    private void InitStats()
    {
        if (isInitialized)
            return;

        isInitialized = true;

        // init statuses
        foreach (Status status in statuses)
        {
            status.maxValue = 100;
            status.currentValue = 100;
        }

        // init attributes
        foreach (Attribute attribute in attributes)
        {
            attribute.baseValue = 10;
            attribute.modifiedValue = 10;
            attribute.exp = 0;
        }

        // init conditions
        foreach (Condition condition in conditions)
        {
            condition.isActive = false;
        }
    }

    public void AddStatusValue(StatusType type, int value)
    {
        int updatedValue = statuses[(int)type].currentValue + value;
        updatedValue = Math.Clamp(updatedValue, 0, statuses[(int)type].maxValue);
        Debug.Log(type.ToString() + ": " + updatedValue);
        statuses[(int)type].currentValue = updatedValue;

        OnStatChanged?.Invoke(this);
    }

    public void AddAttributeValue(AttributeType type, int value)
    {
        if (value < 0) return;

        attributes[(int)type].modifiedValue += value;

        if (type == AttributeType.CON)
        {
            CalulateConstitutionEffects();
        }
        OnStatChanged?.Invoke(this);
    }

    public void AddAttributeExp(AttributeType type, float value)
    {
        if (value < 0) return;

        attributes[(int)type].exp += value;
        if (attributes[(int)type].exp >= 100)
        {
            attributes[(int)type].baseValue++;
            attributes[(int)type].modifiedValue++;
            attributes[(int)type].exp -= 100;
            if (type == AttributeType.CON)
            {
                CalulateConstitutionEffects();
            }
            OnStatChanged?.Invoke(this);
        }
    }

    public void ActivateCondition(Condition condition, float activationTime)
    {
        Debug.Log("Activate " + condition.type + ", " + activationTime);
        condition.isActive = true;
        condition.activationTime += activationTime;
        OnConditionChanged?.Invoke(this, condition);
    }

    public void DeactivateCondition(Condition condition)
    {
        Debug.Log("Deactivate " + condition.type);
        condition.isActive = false;
        condition.activationTime = 0;
        OnConditionChanged?.Invoke(this, condition);
    }

    // calculate HP and SP value when CON changed
    private void CalulateConstitutionEffects()
    {
        int oldValue = statuses[(int)StatusType.HP].maxValue;
        int newValue = statuses[(int)StatusType.HP].maxValue + (5 * (attributes[(int)AttributeType.CON].modifiedValue - 10));

        statuses[(int)StatusType.HP].maxValue = newValue;
        statuses[(int)StatusType.HP].currentValue += newValue - oldValue;

        oldValue = statuses[(int)StatusType.SP].maxValue;
        newValue = statuses[(int)StatusType.SP].maxValue + (2 * (attributes[(int)AttributeType.CON].modifiedValue - 10));

        statuses[(int)StatusType.SP].maxValue = newValue;
        statuses[(int)StatusType.SP].currentValue += newValue - oldValue;
    }
}
