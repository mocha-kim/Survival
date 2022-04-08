using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStatsUI : UserInterface
{
    [SerializeField]
    private StatsObject playerStats;

    [SerializeField]
    private GameObject statusArea;
    private TextMeshProUGUI[] statusTexts;

    [SerializeField]
    private GameObject attributeArea;
    private TextMeshProUGUI[] attributeTexts;
    private Slider[] sliders;

    [SerializeField]
    private GameObject conditionArea;

    private void OnEnable()
    {
        playerStats.OnStatChanged += OnStatChanged;
        playerStats.OnConditionChanged += OnConditionChanged;

        UpdateStatusValues();
        UpdateAttributeValues();
    }

    private void OnDisable()
    {
        playerStats.OnStatChanged -= OnStatChanged;
        playerStats.OnConditionChanged -= OnConditionChanged;
    }

    protected override void Awake()
    {
        base.Awake();

        statusTexts = statusArea.GetComponentsInChildren<TextMeshProUGUI>();
        attributeTexts = attributeArea.GetComponentsInChildren<TextMeshProUGUI>();

        sliders = attributeArea.GetComponentsInChildren<Slider>();

        InitConditionSlots();
    }

    private void Update()
    {
        GameObject slot;
        for (int i = 0; i < playerStats.conditions.Count; i++)
        {
            if (playerStats.conditions[(ConditionType)i].isActive && !playerStats.conditions[(ConditionType)i].needTreatment)
            {
                slot = conditionArea.transform.GetChild(i).gameObject;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = playerStats.conditions[(ConditionType)i].DisplayTime.ToString("f0");
            }
        }
    }

    private void OnStatChanged(StatsObject playerStat)
    {
        UpdateStatusValues();
        UpdateAttributeValues();
    }

    public void OnConditionChanged(StatsObject obj, Condition condition)
    {
        int index = -1;
        for (int i = 0; i < obj.conditions.Count; i++)
        {
            if (obj.conditions[(ConditionType)i].type == condition.type)
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            GameObject slot = conditionArea.transform.GetChild(index).gameObject;
            UpdataConditionSlot(slot, condition);
        }
    }

    private void InitConditionSlots()
    {
        GameObject slot;
        int index = 0;

        foreach (Condition condition in playerStats.conditions.Values)
        {
            slot = conditionArea.transform.GetChild(index).gameObject;
            slot.GetComponentsInChildren<Image>()[1].sprite = condition.icon;
            UpdataConditionSlot(slot, condition);
            index++;
        }
    }

    private void UpdataConditionSlot(GameObject slot, Condition condition)
    {
        slot.GetComponentsInChildren<Image>()[1].color = condition.isActive ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
        slot.GetComponentInChildren<TextMeshProUGUI>().color = (condition.isActive && !condition.needTreatment) ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
    }

    private void UpdateStatusValues()
    {
        for (int i = 0; i < 4; i++)
        {
            statusTexts[i].text = playerStats.statuses[(StatusType)i].currentValue.ToString("n0");
            statusTexts[i + 4].text = playerStats.statuses[(StatusType)i].maxValue.ToString("n0");
        }
    }

    private void UpdateAttributeValues()
    {
        int mValue;
        int bValue;
        for (int i = 0; i < 3; i++)
        {
            mValue = playerStats.attributes[(AttributeType)i].modifiedValue;
            bValue = playerStats.attributes[(AttributeType)i].baseValue;
            attributeTexts[i].text = mValue.ToString();
            attributeTexts[i + 3].text = "( " + bValue.ToString("f0") + (mValue - bValue >= 0 ? " + " : " - ") + Mathf.Abs(mValue - bValue).ToString("f0") + " )";
        }
        sliders[0].value = playerStats.attributes[AttributeType.Handiness].modifiedValue;
        sliders[1].value = playerStats.attributes[AttributeType.Cooking].modifiedValue;
    }
}
