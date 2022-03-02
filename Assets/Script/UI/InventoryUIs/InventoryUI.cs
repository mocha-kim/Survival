using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    public InventoryObject inventoryObject;
    public Dictionary<GameObject, InventorySlot> slots = new();

    [SerializeField]
    protected Sprite[] slotBackgounds;
    protected GameObject previousSlot;

    [SerializeField]
    protected GameObject description;
    protected bool isDescriptionOpened = false;

    protected virtual void Awake()
    {
        CreateSlots();

        // inventory related events
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInventory(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInventory(gameObject); });
    }

    protected virtual void Start()
    {
        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }

    protected abstract void CreateSlots();

    protected virtual void AddEvent(GameObject go, EventTriggerType triggerType, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger)
        {
            Debug.LogWarning("No EventTrigger component found");
            return;
        }

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = triggerType };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    protected virtual void UpdateDescriptions(InventorySlot slot)
    {
        TextMeshProUGUI[] texts = description.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = slot.item.name;
        texts[1].text = slot.ItemObject.EffectsToString();
        texts[2].text = slot.ItemObject.description;
    }

    public virtual void EnableDescription()
    {
        isDescriptionOpened = true;
        description.SetActive(true);
    }

    public virtual void DisableDescription()
    {
        isDescriptionOpened = false;
        description.SetActive(false);
    }

    public void OnPostUpdate(InventorySlot slot)
    {
        slot.slotUI.transform.GetChild(1).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(1).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString());
    }

    public void OnEnterInventory(GameObject go)
    {
        MouseData.mouseHoveredInventory = go.GetComponent<InventoryUI>();
    }

    public void OnExitInventory(GameObject go)
    {
        MouseData.mouseHoveredInventory = null;
    }

    public virtual void OnEnterSlot(GameObject go)
    {
        MouseData.mouseHoverdSlot = go;
    }

    public void OnExitSlot(GameObject go)
    {
        MouseData.mouseHoverdSlot = null;
    }

    public void OnStartDrag(GameObject go)
    {
        MouseData.draggingItem = CreateDragImage(go);
    }

    public void OnDrag(GameObject go)
    {
        if (MouseData.draggingItem == null) return;

        MouseData.draggingItem.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.draggingItem);

        if (MouseData.mouseHoveredInventory == null)
        {
            slots[go].RemoveItem();
        }
        else if (MouseData.mouseHoverdSlot != null)
        {
            InventorySlot mouseHoverSlotData = MouseData.mouseHoveredInventory.slots[MouseData.mouseHoverdSlot];
            inventoryObject.SwapItems(slots[go], mouseHoverSlotData);
        }
    }

    private GameObject CreateDragImage(GameObject go)
    {
        if (slots[go].item.id < 0) return null;

        GameObject dragImageGO = new GameObject();

        RectTransform rect = dragImageGO.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector3(50, 50);
        dragImageGO.transform.SetParent(transform.parent);

        Image image = dragImageGO.AddComponent<Image>();
        image.sprite = slots[go].ItemObject.icon;
        image.raycastTarget = false;

        dragImageGO.name = "Drag Image";

        return dragImageGO;
    }

    public void OnClick(GameObject go, PointerEventData data)
    {
        InventorySlot slot = slots[go];
        if (slot == null) return;

        if (previousSlot != null)
        {
            previousSlot.GetComponentInChildren<Image>().sprite = slotBackgounds[0];
            previousSlot = null;
        }

        if (slot.item.id == -1)
        {
            DisableDescription();
            return;
        }

        previousSlot = go;
        go.GetComponentInChildren<Image>().sprite = slotBackgounds[1];
        Debug.Log(this + " OnClick " + slot.item.name);

        if (data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }

        if (data.button == PointerEventData.InputButton.Right)
        {
            OnRightClick(slot);
        }
    }

    private void OnLeftClick(InventorySlot slot)
    {
        EnableDescription();
        UpdateDescriptions(slot);
    }

    private void OnRightClick(InventorySlot slot)
    {
        inventoryObject.UseItem(slot);
    }
}
