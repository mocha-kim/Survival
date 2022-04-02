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

    private GameObject previousSlot = null;
    private GameObject previousTab = null;
    [SerializeField]
    private Sprite[] tabBackgrounds;

    [SerializeField]
    private GameObject defaultTab;

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
        previousTab = defaultTab;
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
            CreateItem(QuestManager.Instance.acceptedQuests.questObjects[i]);
        }
        for (int i = 0; i < QuestManager.Instance.rewardedQuests.Count; i++)
        {
            CreateItem(QuestManager.Instance.rewardedQuests.questObjects[i]);
        }

        ResizeContent();
    }

    private void CreateItem(QuestObject quest)
    {
        GameObject newItem = Instantiate(slotPrefab, slotParent.transform);
        newItem.name += " " + slotCount;

        AddEvent(newItem, EventTriggerType.PointerClick, (data) => { OnClickSlot(newItem, (PointerEventData)data); });

        float y = start + (-(space + slotHeight) * slotCount);
        newItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);

        newItem.GetComponent<QuestSlotUI>().quest = quest;
        newItem.GetComponent<QuestSlotUI>().UpdateTexts();
        newItem.GetComponent<QuestSlotUI>().UpdateSlotStyle();

        slots.Add(quest, newItem);
        slotCount++;
    }

    public void OnUpdateQuestStatus(QuestObject quest, bool flag)
    {
        if (flag)
        {
            CreateItem(quest);

            ResizeContent();
            RefreshAllItemsTransform();
        }
    }

    public void OnRewardedQuest(QuestObject quest)
    {
        GameObject go = slots[quest];

        go.transform.SetAsLastSibling();
        RefreshAllItemsTransform();
    }

    private void RefreshAllItemsTransform()
    {
        int count = 0;

        foreach (QuestObject quest in QuestManager.Instance.acceptedQuests.questObjects)
        {
            GameObject go = slots[quest];
            if (go.activeSelf)
            {
                float y = start + (-(space + slotHeight) * count);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);

                count++;
            }
        }
        foreach (QuestObject quest in QuestManager.Instance.rewardedQuests.questObjects)
        {
            GameObject go = slots[quest];
            if (go.activeSelf)
            {
                float y = start + (-(space + slotHeight) * count);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);

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
            if (pair.Key.camp == camp)
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

    private void OnClickSlot(GameObject go, PointerEventData data)
    {
        OnClickInterface();

        QuestObject slot = go.GetComponent<QuestSlotUI>().quest;
        if (slot == null)
        {
            return;
        }

        if (previousSlot && (previousSlot == go))
        {
            description.SetActive(false);
            previousSlot = null;
            return;
        }

        description.SetActive(true);
        UpdateDescriptions(slot);

        previousSlot = go;
    }

    public void OnClickAllCamp(GameObject button)
    {
        if (button == previousTab) return;

        if (previousTab)
        {
            previousTab.GetComponent<Image>().sprite = tabBackgrounds[0];
        }
        button.GetComponent<Image>().sprite = tabBackgrounds[1];
        previousTab = button;

        ShowAllItems();
    }

    public void OnClickLawfulCamp(GameObject button)
    {
        if (button == previousTab) return;

        if (previousTab)
        {
            previousTab.GetComponent<Image>().sprite = tabBackgrounds[0];
        }
        button.GetComponent<Image>().sprite = tabBackgrounds[1];
        previousTab = button;

        ShowFilteredItems(QuestCampType.Lawful);
    }

    public void OnClickNeutralCamp(GameObject button)
    {
        if (button == previousTab) return;

        if (previousTab)
        {
            previousTab.GetComponent<Image>().sprite = tabBackgrounds[0];
        }
        button.GetComponent<Image>().sprite = tabBackgrounds[1];
        previousTab = button;

        ShowFilteredItems(QuestCampType.Neutral);
    }

    public void OnClickChaoticCamp(GameObject button)
    {
        if (button == previousTab) return;

        if (previousTab)
        {
            previousTab.GetComponent<Image>().sprite = tabBackgrounds[0];
        }
        button.GetComponent<Image>().sprite = tabBackgrounds[1];
        previousTab = button;

        ShowFilteredItems(QuestCampType.Chaotic);
    }
}
