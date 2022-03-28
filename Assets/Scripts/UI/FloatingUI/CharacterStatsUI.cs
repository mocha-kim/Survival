using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStatsUI : Interface
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
        for (int i = 0; i < playerStats.conditions.Length; i++)
        {
            if (playerStats.conditions[i].isActive)
            {
                slot = conditionArea.transform.GetChild(i).gameObject;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = playerStats.conditions[i].DisplayTime.ToString("n0");
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
        for (int i = 0; i < obj.conditions.Length; i++)
        {
            if (obj.conditions[i].type == condition.type)
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
        for (int i = 0; i < playerStats.conditions.Length; i++)
        {
            slot = conditionArea.transform.GetChild(i).gameObject;
            slot.GetComponentsInChildren<Image>()[1].sprite = playerStats.conditions[i].icon;
            UpdataConditionSlot(slot, playerStats.conditions[i]);
        }
    }

    private void UpdataConditionSlot(GameObject slot, Condition condition)
    {
        slot.GetComponentsInChildren<Image>()[1].color = condition.isActive ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
        slot.GetComponentInChildren<TextMeshProUGUI>().color = condition.isActive ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
    }

    private void UpdateStatusValues()
    {
        for (int i = 0; i < 4; i++)
        {
            statusTexts[i].text = playerStats.statuses[i].currentValue.ToString();
            statusTexts[i + 4].text = playerStats.statuses[i].maxValue.ToString();
        }
    }

    private void UpdateAttributeValues()
    {
        int mValue;
        int bValue;
        for (int i = 0; i < 3; i++)
        {
            mValue = playerStats.attributes[i].modifiedValue;
            bValue = playerStats.attributes[i].baseValue;
            attributeTexts[i].text = mValue.ToString();
            attributeTexts[i + 3].text = "( " + bValue.ToString() + " + " + (mValue - bValue).ToString() + " )";
        }
        sliders[0].value = playerStats.attributes[3].modifiedValue;
        sliders[1].value = playerStats.attributes[4].modifiedValue;
    }
}
