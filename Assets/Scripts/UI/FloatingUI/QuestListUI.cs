using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestListUI : Interface
{
    [SerializeField]
    private GameObject slotParent;
    [SerializeField]
    private GameObject slotPrefab;
    private float slotHeight;

    public Dictionary<QuestObject, GameObject> slots = new();

    [SerializeField]
    private float start;
    [SerializeField]
    private float space;

    [SerializeField]
    private GameObject description;

    private int slotCount = 0;

    protected override void Awake()
    {
        base.Awake();

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(); });
    }

    private void Start()
    {
        QuestManager.Instance.OnUpdateQuestStatus += OnUpdateQuestStatus;
        QuestManager.Instance.OnRewardedQuest += OnRewardedQuest;

        CreateAllItems();
    }

    private void ResizeContent()
    {
        // calc lengthes to resize content area
        float contentLength = Mathf.Abs(start) + slotCount * (slotHeight + space);

        slotParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentLength);
    }

    private void CreateAllItems()
    {
        slotCount = 0;
        slotHeight = slotPrefab.GetComponent<RectTransform>().sizeDelta.y;

        for (int i = 0; i < QuestManager.Instance.acceptedQuests.Count; i++)
        {
            GameObject newItem = Instantiate(slotPrefab, slotParent.transform);
            newItem.name += " " + slotCount;

            AddEvent(newItem, EventTriggerType.PointerClick, (data) => { OnClickSlot(newItem, (PointerEventData)data); });

            float y = start + (-(space + slotHeight) * slotCount);
            newItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);

            newItem.GetComponent<QuestSlotUI>().quest = QuestManager.Instance.acceptedQuests.questObjects[i];
            newItem.GetComponent<QuestSlotUI>().UpdateTexts();
            newItem.GetComponent<QuestSlotUI>().UpdateBackground();

            slots.Add(QuestManager.Instance.acceptedQuests.questObjects[i], newItem);
            slotCount++;
        }

        for (int i = 0; i < QuestManager.Instance.rewardedQuests.Count; i++)
        {
            GameObject newItem = Instantiate(slotPrefab, slotParent.transform);
            newItem.name += " " + slotCount;

            AddEvent(newItem, EventTriggerType.PointerClick, (data) => { OnClickSlot(newItem, (PointerEventData)data); });

            float y = start + (-(space + slotHeight) * slotCount);
            newItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);

            newItem.GetComponent<QuestSlotUI>().quest = QuestManager.Instance.rewardedQuests.questObjects[i];
            newItem.GetComponent<QuestSlotUI>().UpdateTexts();
            newItem.GetComponent<QuestSlotUI>().UpdateBackground();

            slots.Add(QuestManager.Instance.rewardedQuests.questObjects[i], newItem);
            slotCount++;
        }

        ResizeContent();
    }

    private void CreateAcceptedItem(QuestObject quest)
    {
        GameObject newItem = Instantiate(slotPrefab, slotParent.transform);
        newItem.name += " " + slotCount;

        AddEvent(newItem, EventTriggerType.PointerClick, (data) => { OnClickSlot(newItem, (PointerEventData)data); });

        float y = start + (-(space + slotHeight) * slotCount);
        newItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);

        newItem.GetComponent<QuestSlotUI>().quest = quest;
        newItem.GetComponent<QuestSlotUI>().UpdateTexts();

        slots.Add(quest, newItem);
        slotCount++;

        ResizeContent();
        RefreshAllItemsTransform();
    }

    public void OnUpdateQuestStatus(QuestObject quest, bool flag)
    {
        if (flag)
        {
            CreateAcceptedItem(quest);
        }

        slots[quest]?.GetComponent<QuestSlotUI>().UpdateBackground();
    }

    public void OnRewardedQuest(QuestObject quest)
    {
        GameObject go = slots[quest];

        go.GetComponent<QuestSlotUI>().UpdateBackground();

        go.transform.SetAsLastSibling();
        RefreshAllItemsTransform();
    }

    private void RefreshAllItemsTransform()
    {
        int count = 0;

        foreach (KeyValuePair<QuestObject, GameObject> pair in slots)
        {
            if (pair.Value.activeSelf)
            {
                float y = start + (-(space + slotHeight) * count);
                pair.Value.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);

                count++;
            }
        }
    }

    private void ShowAllItems()
    {
        slotCount = 0;
        slotHeight = slotPrefab.GetComponent<RectTransform>().sizeDelta.y;

        foreach (KeyValuePair<QuestObject, GameObject> pair in slots)
        {
            pair.Value.SetActive(true);
            slotCount++;
        }

        ResizeContent();
        RefreshAllItemsTransform();
    }

    private void ShowFilteredItems(QuestCampType camp)
    {
        slotCount = 0;
        slotHeight = slotPrefab.GetComponent<RectTransform>().sizeDelta.y;

        foreach (KeyValuePair<QuestObject, GameObject> pair in slots)
        {
            pair.Value.SetActive(false);
            if (pair.Key.camp != camp)
            {
                pair.Value.SetActive(true);
                slotCount++;
            }
        }

        ResizeContent();
        RefreshAllItemsTransform();
    }

    private void UpdateDescriptions(QuestObject quest)
    {
        TextMeshProUGUI[] texts = description.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = quest.title;
        texts[1].text = quest.description;
    }

    private void EnableDescription()
    {
        description.SetActive(true);
    }

    private void DisableDescription()
    {
        description.SetActive(false);
    }

    private void OnClickSlot(GameObject go, PointerEventData data)
    {
        OnClickInterface();

        QuestObject slot = go.GetComponent<QuestSlotUI>().quest;
        if (slot == null)
        {
            return;
        }

        EnableDescription();
        UpdateDescriptions(slot);
    }

    public void OnClickLawfulCamp()
    {
        ShowFilteredItems(QuestCampType.Lawful);
    }

    public void OnClickNeutralCamp()
    {
        ShowFilteredItems(QuestCampType.Neutral);
    }

    public void OnClickChaoticCamp()
    {
        ShowFilteredItems(QuestCampType.Chaotic);
    }

    public void OnClickAllCamp()
    {
        ShowAllItems();
    }
}
