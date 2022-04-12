using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugUI : MonoBehaviour
{
    public StatsObject playerStat;
    public InventoryObject playerInventory;
    public ItemDatabase database;

    public TextMeshProUGUI[] valueTexts;
    public TMP_Dropdown dropdown;
    public Toggle toggle;
    public TMP_InputField input;

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
        playerStat.AddStatusCurrentValue(StatusType.HP, 10);
        playerStat.AddStatusCurrentValue(StatusType.SP, 10);
        playerStat.AddStatusCurrentValue(StatusType.Hunger, 10);
        playerStat.AddStatusCurrentValue(StatusType.Thirst, 10);
    }

    public void OnClickSDown()
    {
        playerStat.AddStatusCurrentValue(StatusType.HP, -10);
        playerStat.AddStatusCurrentValue(StatusType.SP, -10);
        playerStat.AddStatusCurrentValue(StatusType.Hunger, -10);
        playerStat.AddStatusCurrentValue(StatusType.Thirst, -10);
    }

    public void OnClickAMUp()
    {
        playerStat.AddAttributeModifiedValue(AttributeType.CON, 1);
        playerStat.AddAttributeModifiedValue(AttributeType.STR, 1);
        playerStat.AddAttributeModifiedValue(AttributeType.DEF, 1);
    }

    public void OnClickAMDown()
    {
        playerStat.AddAttributeModifiedValue(AttributeType.CON, -1);
        playerStat.AddAttributeModifiedValue(AttributeType.STR, -1);
        playerStat.AddAttributeModifiedValue(AttributeType.DEF, -1);
    }

    public void OnClickABUp()
    {
        playerStat.AddAttributeBaseValue(AttributeType.CON, 1);
        playerStat.AddAttributeBaseValue(AttributeType.STR, 1);
        playerStat.AddAttributeBaseValue(AttributeType.DEF, 1);
        playerStat.AddAttributeBaseValue(AttributeType.Handiness, 1);
        playerStat.AddAttributeBaseValue(AttributeType.Cooking, 1);
    }

    public void OnClickAExpUp()
    {
        playerStat.AddAttributeExp(AttributeType.CON, 10);
        playerStat.AddAttributeExp(AttributeType.STR, 10);
        playerStat.AddAttributeExp(AttributeType.DEF, 10);
        playerStat.AddAttributeExp(AttributeType.Handiness, 10);
        playerStat.AddAttributeExp(AttributeType.Cooking, 10);
    }

    public void OnClickResetStats()
    {
        for (StatusType type = StatusType.HP; type <= StatusType.Thirst; type++)
        {
            playerStat.statuses[type].maxValue = 100;
            playerStat.statuses[type].currentValue = 100;
        }
        for (AttributeType type = AttributeType.CON; type <= AttributeType.DEF; type++)
        {
            playerStat.attributes[type].baseValue = 10;
            playerStat.attributes[type].modifiedValue = 10;
            playerStat.attributes[type].exp = 0;
        }
        for (AttributeType type = AttributeType.Handiness; type <= AttributeType.Cooking; type++)
        {
            playerStat.attributes[type].baseValue = 1;
            playerStat.attributes[type].modifiedValue = 1;
            playerStat.attributes[type].exp = 0;
        }
        for (ConditionType type = ConditionType.SwallowWound; type <= ConditionType.MentalBreakdown; type++)
        {
            playerStat.DeactivateCondition(type);
        }
    }

    public void OnClickAdd()
    {
        foreach (Condition condition in playerStat.conditions.Values)
        {
            if (condition.type == (ConditionType)dropdown.value)
            {
                if (toggle.isOn)
                {
                    playerStat.ActivateCondition(condition);
                }
                else if (float.TryParse(input.text, out float inputNum))
                {
                    playerStat.ActivateCondition(condition, inputNum);
                }
                return;
            }
        }
    }

    public void OnClickSub()
    {
        foreach (Condition condition in playerStat.conditions.Values)
        {
            if (condition.type == (ConditionType)dropdown.value)
            {
                if (!toggle.isOn)
                {
                    playerStat.DeactivateCondition(condition);
                }
                else if (float.TryParse(input.text, out float inputNum))
                {
                    playerStat.ActivateCondition(condition, -inputNum);
                }
                return;
            }
        }
    }

    public void OnClickPotatoBtn()
    {
        playerInventory.AddItem(database.GetItem(0), 1);
    }

    public void OnClickBatBtn()
    {
        playerInventory.AddItem(database.GetItem(1), 1);
    }

    public void OnClickCookbookBtn()
    {
        playerInventory.AddItem(database.GetItem(2), 1);
    }

    public void OnClickAcceptBtn()
    {
        if (acceptIdx >= QuestManager.Instance.questDatabase.data.Count)
        {
            return;
        }
        QuestManager.Instance.UpdateQuestStatus(QuestManager.Instance.questDatabase.data[acceptIdx], QuestStatus.Accepted);
        acceptIdx++;
    }

    public void OnClickRewardBtn()
    {
        if (rewardIdx >= QuestManager.Instance.questDatabase.data.Count)
        {
            return;
        }
        QuestManager.Instance.UpdateQuestStatus(QuestManager.Instance.questDatabase.data[rewardIdx], QuestStatus.Rewarded);
        rewardIdx++;
    }
}
