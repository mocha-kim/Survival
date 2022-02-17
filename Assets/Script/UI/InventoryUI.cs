using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(EventTrigger))]
public class InventoryUI : MonoBehaviour
{
    public InventoryObject inventoryObject;
    public Dictionary<GameObject, InventorySlot> slots = new();

    [SerializeField]
    private GameObject slotParent;
    [SerializeField]
    private bool needToResize = false;
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Vector2 start;
    [SerializeField]
    private Vector2 space;
    [SerializeField]
    private int colNum;
    private float size;

    private void Awake()
    {
        CreateSlots();

        // inventory related events
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInventory(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInventory(gameObject); });
    }

    private void Start()
    {
        if (needToResize)
        {
            float y = 2 * Mathf.Abs(start.y) - size + (inventoryObject.Slots.Length / colNum) * (size + space.y) - space.y;
            slotParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, y);
        }

        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }

    private void CreateSlots()
    {
        size = slotPrefab.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent.transform);
            newSlot.name += ": " + i;

            float x = start.x + ((space.x + size) * (i % colNum));
            float y = start.y + (-(space.y + size) * (i / colNum));
            newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0f);

            // slot related events
            AddEvent(newSlot, EventTriggerType.PointerEnter, delegate { OnEnterSlot(newSlot); });
            AddEvent(newSlot, EventTriggerType.PointerExit, delegate { OnExitSlot(newSlot); });
            AddEvent(newSlot, EventTriggerType.BeginDrag, delegate { OnStartDrag(newSlot); });
            AddEvent(newSlot, EventTriggerType.Drag, delegate { OnDrag(newSlot); });
            AddEvent(newSlot, EventTriggerType.EndDrag, delegate { OnEndDrag(newSlot); });
            AddEvent(newSlot, EventTriggerType.PointerClick, (data) => { OnClick(newSlot, (PointerEventData)data); });

            inventoryObject.Slots[i].slotUI = newSlot;
            inventoryObject.Slots[i].parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;

            slots.Add(newSlot, inventoryObject.Slots[i]);
        }
    }

    private void AddEvent(GameObject go, EventTriggerType triggerType, UnityAction<BaseEventData> action)
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

    public void OnEnterSlot(GameObject go)
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

    }

    private void OnRightClick(InventorySlot slot)
    {
        inventoryObject.UseItem(slot);
    }
}
