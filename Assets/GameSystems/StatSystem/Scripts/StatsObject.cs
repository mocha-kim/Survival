using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Stat System/Stats")]
public class StatsObject : ScriptableObject
{
    public SerializableDictionary<StatusType, Status> statuses = new();
    public SerializableDictionary<AttributeType, Attribute> attributes = new();
    public SerializableDictionary<ConditionType, Condition> conditions = new();

    public Action<StatsObject> OnStatChanged;
    public Action<StatsObject, Condition> OnConditionChanged;

    public float lawfulCoin;
    public float neutralCoin;
    public float chaoticCoin;

    public bool isInitialized = false;
    public bool IsDead => statuses[StatusType.HP].currentValue <= 0;

    public int CountActivatedConditions
    {
        get
        {
            int count = 0;
            foreach (Condition condition in conditions.Values)
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
        {
            return;
        }

        isInitialized = true;

        // init statuses
        for (StatusType type = StatusType.HP; type <= StatusType.Thirst; type++)
        {
            Status status = new(type, 100);
            statuses[type] = status;

            Debug.Log("created " + type.ToString());
        }

        // init attributes
        for (AttributeType type = AttributeType.CON; type <= AttributeType.DEF; type++)
        {
            Attribute attribute = new(type, 10);
            attributes[type] = attribute;
            Debug.Log("created " + type.ToString());
        }
        for (AttributeType type = AttributeType.Handiness; type <= AttributeType.Cooking; type++)
        {
            Attribute attribute = new(type, 1);
            attributes[type] = attribute;
            Debug.Log("created " + type.ToString());
        }

        // init conditions
        for (ConditionType type = ConditionType.SwallowWound; type <= ConditionType.MentalBreakdown; type++)
        {
            Condition condition = new(type);
            conditions[type] = condition;
            Debug.Log("created " + type.ToString());
        }
    }

    public void AddStatusValue(StatusType type, int value)
    {
        int updatedValue = statuses[type].currentValue + value;
        updatedValue = Math.Clamp(updatedValue, 0, statuses[type].maxValue);
        statuses[type].currentValue = updatedValue;

        OnStatChanged?.Invoke(this);
    }

    public void AddAttributeValue(AttributeType type, int value)
    {
        if (value < 0)
        {
            return;
        }

        attributes[type].modifiedValue += value;

        if (type == AttributeType.CON)
        {
            CalulateConstitutionEffects();
        }
        OnStatChanged?.Invoke(this);
    }

    public void AddAttributeExp(AttributeType type, float value)
    {
        if (value < 0)
        {
            return;
        }

        attributes[type].exp += value;
        if (attributes[type].exp >= 100)
        {
            attributes[type].baseValue++;
            attributes[type].modifiedValue++;
            attributes[type].exp -= 100;
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

    public void ActivateCondition(ConditionType type, float activationTime)
    {
        Debug.Log("Activate " + type + ", " + activationTime);

        Condition condition = null;

        foreach (Condition c in conditions.Values)
        {
            if (c.type == type)
            {
                condition = c;
                break;
            }
        }
        if (condition == null)
        {
            return;
        }

        condition.isActive = true;
        condition.activationTime += activationTime;
        OnConditionChanged?.Invoke(this, condition);
    }

    public void DeactivateCondition(ConditionType type)
    {
        Debug.Log("Deactivate " + type);

        Condition condition = null;

        foreach (Condition c in conditions.Values)
        {
            if (c.type == type)
            {
                condition = c;
                break;
            }
        }
        if (condition == null)
        {
            return;
        }

        condition.isActive = false;
        condition.activationTime = 0;
        OnConditionChanged?.Invoke(this, condition);
    }

    // calculate HP and SP value when CON changed
    private void CalulateConstitutionEffects()
    {
        int oldValue = statuses[StatusType.HP].maxValue;
        int newValue = statuses[StatusType.HP].maxValue + (5 * (attributes[(int)AttributeType.CON].modifiedValue - 10));

        statuses[StatusType.HP].maxValue = newValue;
        statuses[StatusType.HP].currentValue += newValue - oldValue;

        oldValue = statuses[StatusType.SP].maxValue;
        newValue = statuses[StatusType.SP].maxValue + (2 * (attributes[(int)AttributeType.CON].modifiedValue - 10));

        statuses[StatusType.SP].maxValue = newValue;
        statuses[StatusType.SP].currentValue += newValue - oldValue;
    }
}
