using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public abstract class InventoryUI : UserInterface
{
    public InventoryObject inventoryObject;
    public Dictionary<GameObject, InventorySlot> slots = new();

    [SerializeField]
    protected GameObject description;
    protected bool isDescriptionOpened = false;

    protected virtual void OnEnable()
    {
        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }

    protected override void Awake()
    {
        CreateSlots();

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInventory(); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInventory(); });

        base.Awake();
    }

    public void OnPostUpdate(InventorySlot slot)
    {
        slot.slotUI.transform.GetChild(1).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(1).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString());
    }

    public void OnEnterInventory()
    {
        MouseData.mouseHoveredInventory = gameObject.GetComponent<InventoryUI>();
        OnEnterInterface();
    }

    public void OnExitInventory()
    {
        MouseData.mouseHoveredInventory = null;
        OnExitInterface();
    }

    public virtual void OnEnterSlot(GameObject go)
    {
        MouseData.mouseHoverdSlot = go;
    }

    public void OnExitSlot(GameObject go)
    {
        MouseData.mouseHoverdSlot = null;
    }

    public void OnStartDragItem(GameObject go)
    {
        MouseData.draggingItem = CreateDragItemImage(go);
    }

    public void OnDragItem(GameObject go)
    {
        if (MouseData.draggingItem == null)
        {
            return;
        }

        MouseData.draggingItem.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDragItem(GameObject go)
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

    public abstract void OnClickSlot(GameObject go, PointerEventData data);

    protected abstract void CreateSlots();

    private GameObject CreateDragItemImage(GameObject go)
    {
        if (slots[go].item.id < 0)
        {
            return null;
        }

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

    protected virtual void UpdateDescriptions(InventorySlot slot)
    {
        TextMeshProUGUI[] texts = description.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = slot.item.name;
        texts[1].text = slot.ItemObject.EffectsToString();
        texts[2].text = slot.ItemObject.description;
    }

    protected virtual void EnableDescription()
    {
        isDescriptionOpened = true;
        description.SetActive(true);
    }

    protected virtual void DisableDescription()
    {
        isDescriptionOpened = false;
        description.SetActive(false);
    }
}
