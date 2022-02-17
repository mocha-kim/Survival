using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDebugUI : MonoBehaviour
{
    public StatsObject playerStat;
    public TextMeshProUGUI[] valueTexts;

    private void OnEnable()
    {
        playerStat.OnStatChanged += OnStatChanged;

        UpdateValueTexts();
    }

    private void OnDisable()
    {
        playerStat.OnStatChanged -= OnStatChanged;
    }

    private void UpdateValueTexts()
    {
        valueTexts[0].text = (playerStat.statuses[0].currentValue).ToString() + "/" + (playerStat.statuses[0].maxValue).ToString();
        valueTexts[1].text = (playerStat.statuses[1].currentValue).ToString() + "/" + (playerStat.statuses[1].maxValue).ToString();
        valueTexts[2].text = (playerStat.statuses[2].currentValue).ToString() + "/" + (playerStat.statuses[2].maxValue).ToString();
        valueTexts[3].text = (playerStat.statuses[3].currentValue).ToString() + "/" + (playerStat.statuses[3].maxValue).ToString();

        valueTexts[4].text = playerStat.attributes[0].modifiedValue.ToString();
        valueTexts[5].text = playerStat.attributes[1].modifiedValue.ToString();
        valueTexts[6].text = playerStat.attributes[2].modifiedValue.ToString();
        valueTexts[7].text = playerStat.attributes[3].modifiedValue.ToString();
        valueTexts[8].text = playerStat.attributes[4].modifiedValue.ToString();
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
}
