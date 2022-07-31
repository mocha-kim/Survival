using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPocketUI : InventoryUI
{
    private static ItemPocketUI instance;
    public static ItemPocketUI Instance => instance;

    private GameObject previousSlot;

    [SerializeField]
    protected Sprite[] slotBackgounds;

    [SerializeField]
    private GameObject[] staticSlots;

    protected override void Awake()
    {
        instance = this;
        base.Awake();
    }

    protected override void OnEnable()
    {
        if (inventoryObject)
            base.OnEnable();
    }

    protected override void CreateSlots()
    {
        // item pocket's slots are created(static)
        for (int i = 0; i < staticSlots.Length; ++i)
        {
            GameObject slotUI = staticSlots[i];

            AddEvent(slotUI, EventTriggerType.PointerEnter, delegate { OnEnterSlot(slotUI); });
            AddEvent(slotUI, EventTriggerType.PointerExit, delegate { OnExitSlot(slotUI); });
            AddEvent(slotUI, EventTriggerType.BeginDrag, delegate { OnStartDragItem(slotUI); });
            AddEvent(slotUI, EventTriggerType.Drag, delegate { OnDragItem(slotUI); });
            AddEvent(slotUI, EventTriggerType.EndDrag, delegate { OnEndDragItem(slotUI); });
        }
        // we need to link slots and slotUIs every time when inventory object changes
    }

    private void UpdateSlotLinks()
    {
        slots.Clear();
        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            GameObject slotUI = staticSlots[i];

            inventoryObject.Slots[i].slotUI = slotUI;
            inventoryObject.Slots[i].parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;

            slots.Add(slotUI, inventoryObject.Slots[i]);
        }
    }

    private void UpdateSlots()
    {
        foreach (InventorySlot slot in inventoryObject.Slots)
            OnPostUpdate(slot);
    }

    public void SetInventoryObject(InventoryObject inventory)
    {
        inventoryObject = inventory;
        UpdateSlotLinks();
        UpdateSlots();
    }

    public override void OnClickSlot(GameObject go, PointerEventData data)
    {
        OnClickInterface();

        InventorySlot slot = slots[go];
        if (slot == null)
        {
            return;
        }

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
