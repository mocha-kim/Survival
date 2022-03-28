using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConditionUI : MonoBehaviour
{
    [SerializeField]
    private StatsObject playerStats;

    [SerializeField]
    private GameObject[] slotGOs;

    private int activatedNum = 0;
    private Dictionary<GameObject, Condition> slots = new();

    private void Start()
    {
        InitSlots();
    }

    private void OnEnable()
    {
        playerStats.OnConditionChanged += OnConditionChanged;
    }

    private void OnDisable()
    {
        playerStats.OnConditionChanged -= OnConditionChanged;
    }

    private void InitSlots()
    {
        for (int i = 0; i < 9; i++)
        {
            slots.Add(slotGOs[i], null);
            slotGOs[i].SetActive(false);
        }

        UpdateConditionInfo();
    }

    private void UpdateConditionInfo()
    {
        activatedNum = 0;
        foreach (Condition condition in playerStats.conditions)
        {
            if (condition.isActive)
            {
                slots[slotGOs[activatedNum]] = condition;

                slotGOs[activatedNum].GetComponentsInChildren<Image>()[1].sprite = condition.icon;
                TextMeshProUGUI[] texts = slotGOs[activatedNum].GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = condition.type.ToString();

                slotGOs[activatedNum].SetActive(true);

                activatedNum++;
            }
        }
    }

    public void OnConditionChanged(StatsObject obj, Condition condition)
    {
        int changedIdx = -1;
        for (int i = 0; i < activatedNum; i++)
        {
            if (slots[slotGOs[i]] == condition)
            {
                changedIdx = i;
                break;
            }
        }

        Debug.Log("Changed condition: " + condition.type + "(" + changedIdx + ")");

        if (condition.isActive)
        {
            if (changedIdx == -1)
            {
                slots[slotGOs[activatedNum]] = condition;
                slotGOs[activatedNum].SetActive(true);
            }
        }
        else
        {
            if (changedIdx == -1) return;

            for (int i = changedIdx + 1; i < activatedNum; i++)
            {
                slots[slotGOs[i - 1]] = slots[slotGOs[i]];
            }
            slotGOs[activatedNum - 1].SetActive(false);
            slots[slotGOs[activatedNum - 1]] = null;
        }
        UpdateConditionInfo();
    }
}