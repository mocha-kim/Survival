using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    public StatsObject playerStat;
    public InventoryObject playerInventory;
    public ItemDatabase database;

    public TextMeshProUGUI[] valueTexts;
    public TMP_Dropdown dropdown;
    public TMP_InputField input;

    public QuestObject[] testQuests;
    private int acceptIdx = 0;
    private int rewardIdx = 0;

    private void Awake()
    {
        playerStat.OnStatChanged += OnStatChanged;

        UpdateValueTexts();
        InitConditionTypes();
    }

    private void UpdateValueTexts()
    {
        valueTexts[0].text = (playerStat.statuses[(StatusType)0].currentValue).ToString() + "/" + (playerStat.statuses[(StatusType)0].maxValue).ToString();
        valueTexts[1].text = (playerStat.statuses[(StatusType)1].currentValue).ToString() + "/" + (playerStat.statuses[(StatusType)1].maxValue).ToString();
        valueTexts[2].text = (playerStat.statuses[(StatusType)2].currentValue).ToString() + "/" + (playerStat.statuses[(StatusType)2].maxValue).ToString();
        valueTexts[3].text = (playerStat.statuses[(StatusType)3].currentValue).ToString() + "/" + (playerStat.statuses[(StatusType)3].maxValue).ToString();

        valueTexts[4].text = playerStat.attributes[(AttributeType)0].modifiedValue.ToString();
        valueTexts[5].text = playerStat.attributes[(AttributeType)1].modifiedValue.ToString();
        valueTexts[6].text = playerStat.attributes[(AttributeType)2].modifiedValue.ToString();
        valueTexts[7].text = playerStat.attributes[(AttributeType)3].modifiedValue.ToString();
        valueTexts[8].text = playerStat.attributes[(AttributeType)4].modifiedValue.ToString();
    }

    private void InitConditionTypes()
    {
        dropdown.options.Clear();
        for (int i = 0; i < 9; i++)
        {
            TMP_Dropdown.OptionData option = new();
            option.text = ((ConditionType)i).ToString();
            dropdown.options.Add(option);
        }
    }

    private void OnStatChanged(StatsObject playerStat)
    {
        UpdateValueTexts();
    }

    public void OnClickSUp()
    {
        playerStat.AddStatusValue(StatusType.HP, 10);
        playerStat.AddStatusValue(StatusType.SP, 10);
        playerStat.AddStatusValue(StatusType.Hunger, 10);
        playerStat.AddStatusValue(StatusType.Thirst, 10);
    }

    public void OnClickSDown()
    {
        playerStat.AddStatusValue(StatusType.HP, -10);
        playerStat.AddStatusValue(StatusType.SP, -10);
        playerStat.AddStatusValue(StatusType.Hunger, -10);
        playerStat.AddStatusValue(StatusType.Thirst, -10);
    }

    public void OnClickAUp()
    {
        playerStat.AddAttributeValue(AttributeType.CON, 1);
        playerStat.AddAttributeValue(AttributeType.STR, 1);
        playerStat.AddAttributeValue(AttributeType.DEF, 1);
        playerStat.AddAttributeValue(AttributeType.Handiness, 1);
        playerStat.AddAttributeValue(AttributeType.Cooking, 1);
    }

    public void OnClickADown()
    {
        playerStat.AddAttributeValue(AttributeType.CON, -1);
        playerStat.AddAttributeValue(AttributeType.STR, -1);
        playerStat.AddAttributeValue(AttributeType.DEF, -1);
        playerStat.AddAttributeValue(AttributeType.Handiness, -1);
        playerStat.AddAttributeValue(AttributeType.Cooking, -1);
    }

    public void OnClickAdd()
    {
        foreach (Condition condition in playerStat.conditions.Values)
        {
            if (condition.type == (ConditionType)dropdown.value)
            {
                if (float.TryParse(input.text, out float inputNum))
                {
                    playerStat.ActivateCondition(condition, inputNum);
                }
                return;
            }
        }
    }

    public void OnClickBatBtn()
    {
        playerInventory.AddItem(database.GetItem(1), 1);
    }

    public void OnClickPotatoBtn()
    {
        playerInventory.AddItem(database.GetItem(0), 1);
    }

    public void OnClickAcceptBtn()
    {
        if (acceptIdx >= testQuests.Length)
        {
            return;
        }
        QuestManager.Instance.UpdateQuestStatus(testQuests[acceptIdx], QuestStatus.Accepted);
        acceptIdx++;
    }

    public void OnClickRewardBtn()
    {
        if (rewardIdx >= testQuests.Length)
        {
            return;
        }
        QuestManager.Instance.UpdateQuestStatus(testQuests[rewardIdx], QuestStatus.Rewarded);
        rewardIdx++;
    }
}
