using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConditionSlotUI : MonoBehaviour
{
    public StatsObject playerStat;

    public TextMeshProUGUI timeText;
    public Image iconImage;

    private float deactiveTime;
    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= deactiveTime)
        {
            enabled = false;
        }
        Debug.Log("call slot update");
    }

    public void ActivateThisSlot(ConditionType type, Sprite icon)
    {
        deactiveTime = playerStat.conditions[(int)type].activationTime;
        elapsedTime = 0;

        timeText.text = deactiveTime.ToString("n0");
        iconImage.sprite = icon;
    }
}
