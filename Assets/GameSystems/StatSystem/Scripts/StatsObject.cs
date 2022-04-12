using System;
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
        }

        // init attributes
        for (AttributeType type = AttributeType.CON; type <= AttributeType.DEF; type++)
        {
            Attribute attribute = new(type, 10);
            attributes[type] = attribute;
        }
        for (AttributeType type = AttributeType.Handiness; type <= AttributeType.Cooking; type++)
        {
            Attribute attribute = new(type, 1);
            attributes[type] = attribute;
        }

        // init conditions
        for (ConditionType type = ConditionType.SwallowWound; type <= ConditionType.MentalBreakdown; type++)
        {
            Condition condition = new(type);
            conditions[type] = condition;
        }
    }

    public void ResetStats()
    {
        for (StatusType type = StatusType.HP; type <= StatusType.Thirst; type++)
        {
            statuses[type].maxValue = 100;
            statuses[type].currentValue = 100;
        }
        for (AttributeType type = AttributeType.CON; type <= AttributeType.DEF; type++)
        {
            attributes[type].baseValue = 10;
            attributes[type].modifiedValue = 10;
            attributes[type].exp = 0;
        }
        for (AttributeType type = AttributeType.Handiness; type <= AttributeType.Cooking; type++)
        {
            attributes[type].baseValue = 1;
            attributes[type].modifiedValue = 1;
            attributes[type].exp = 0;
        }
        for (ConditionType type = ConditionType.SwallowWound; type <= ConditionType.MentalBreakdown; type++)
        {
            DeactivateCondition(type);
        }
    }

    public int CountActivatedConditions()
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

    public void AddStatusCurrentValue(StatusType type, float value)
    {
        float updatedValue = statuses[type].currentValue + value;
        updatedValue = Math.Clamp(updatedValue, 0, statuses[type].maxValue);
        statuses[type].currentValue = updatedValue;

        OnStatChanged?.Invoke(this);
    }

    // returns updated max value
    public float AddStatusMaxValue(StatusType type, float value)
    {
        if (value <= 0) return statuses[type].maxValue;

        float updatedValue = statuses[type].maxValue + value;
        float limitValue = GetStatusMaxValue(type);

        updatedValue = Math.Clamp(updatedValue, 0, limitValue);
        statuses[type].maxValue = updatedValue;

        OnStatChanged?.Invoke(this);
        return updatedValue;
    }

    // Use when equipments chaged only
    public void AddAttributeBaseValue(AttributeType type, int value)
    {
        if (value <= 0) return;

        int oldValue = attributes[type].baseValue;
        int updatedValue = oldValue + value;
        int limitValue = GetAttributeMaxValue(type);

        updatedValue = Mathf.Clamp(updatedValue, 0, limitValue);
        attributes[type].baseValue = updatedValue;
        AddAttributeModifiedValue(type, updatedValue - oldValue);

        if (type == AttributeType.CON)
        {
            CalulateConstitutionEffects();
        }
        OnStatChanged?.Invoke(this);
    }

    // Cooking, Handiness attributes must not use this
    public void AddAttributeModifiedValue(AttributeType type, int value)
    {
        int updatedValue = attributes[type].modifiedValue + value;

        updatedValue = Mathf.Clamp(updatedValue, 0, 100 - GetAttributeMaxValue(type));
        attributes[type].modifiedValue = updatedValue;

        if (type == AttributeType.CON)
        {
            CalulateConstitutionEffects();
        }
        OnStatChanged?.Invoke(this);
    }

    public void AddAttributeExp(AttributeType type, float value)
    {
        if (value <= 0) return;

        attributes[type].exp += value;
        if (attributes[type].exp >= 100)
        {
            if (attributes[type].baseValue + 1 > GetAttributeMaxValue(type)) return;
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

    public void ActivateCondition(Condition condition)
    {
        Debug.Log("Activate " + condition.type + ", it will last until treated");
        condition.isActive = true;
        condition.needTreatment = true;
        OnConditionChanged?.Invoke(this, condition);
    }

    public void DeactivateCondition(Condition condition)
    {
        Debug.Log("Deactivate " + condition.type);
        condition.isActive = false;
        condition.activationTime = 0;
        condition.needTreatment = false;
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
        float oldValue = statuses[StatusType.HP].maxValue;
        float newValue = AddStatusMaxValue(StatusType.HP, 5 * (attributes[AttributeType.CON].modifiedValue - 10));

        statuses[StatusType.HP].maxValue = newValue;
        statuses[StatusType.HP].currentValue += newValue - oldValue;

        oldValue = statuses[StatusType.SP].maxValue;
        newValue = AddStatusMaxValue(StatusType.SP, 2 * (attributes[AttributeType.CON].modifiedValue - 10));

        statuses[StatusType.SP].maxValue = newValue;
        statuses[StatusType.SP].currentValue += newValue - oldValue;
    }

    private float GetStatusMaxValue(StatusType type)
    {
        switch (type)
        {
            case StatusType.HP:
                return 500;
            case StatusType.SP:
                return 200;
            case StatusType.Hunger:
            case StatusType.Thirst:
                return 100;
        }
        return 0;
    }

    private int GetAttributeMaxValue(AttributeType type)
    {
        switch (type)
        {
            case AttributeType.CON:
            case AttributeType.STR:
            case AttributeType.DEF:
                return 60;
            case AttributeType.Handiness:
            case AttributeType.Cooking:
                return 10;
        }
        return 0;
    }
}
