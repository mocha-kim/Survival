using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickslotsUI : InventoryUI
{
    public GameObject[] staticSlots = null;

    [SerializeField]
    private Sprite[] slotBackgounds;
    private GameObject previousSlot;

    public KeyCode[] quickslots = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

    protected override void CreateSlots()
    {
        slots = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            GameObject slotUI = staticSlots[i];

            AddEvent(slotUI, EventTriggerType.PointerEnter, delegate { OnEnterSlot(slotUI); });
            AddEvent(slotUI, EventTriggerType.PointerExit, delegate { OnExitSlot(slotUI); });
            AddEvent(slotUI, EventTriggerType.BeginDrag, delegate { OnStartDragItem(slotUI); });
            AddEvent(slotUI, EventTriggerType.Drag, delegate { OnDragItem(slotUI); });
            AddEvent(slotUI, EventTriggerType.EndDrag, delegate { OnEndDragItem(slotUI); });

            inventoryObject.Slots[i].slotUI = slotUI;
            inventoryObject.Slots[i].parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;

            slots.Add(slotUI, inventoryObject.Slots[i]);
        }
    }

    private void Start()
    {
        SelectSlot(GameManager.Instance.gameData.selectedQuickslot);
    }

    private void Update()
    {
        for (int i = 0; i < quickslots.Length; i++)
        {
            if (Input.GetKeyDown(quickslots[i]))
            {
                SelectSlot(i);
                UseSlotItem(i);
            }
        }
    }

    private void SelectSlot(int index)
    {
        InventorySlot slot = inventoryObject.Slots[index];
        if (slot == null)
        {
            return;
        }

        if (previousSlot != null)
        {
            previousSlot.GetComponentInChildren<Image>().sprite = slotBackgounds[0];
            previousSlot = null;
        }

        previousSlot = staticSlots[index];
        staticSlots[index].GetComponentInChildren<Image>().sprite = slotBackgounds[1];
    }

    private void UseSlotItem(int index)
    {
        InventorySlot slot = inventoryObject.Slots[index];
        if (slot == null || slot.item.id == -1)
        {
            return;
        }

        // quickslot only allows equipment, food, treatment type
        if (slot.ItemObject.type == ItemType.Equipment)
        {

        }
        else
        {
            inventoryObject.UseItem(inventoryObject.Slots[index]);
        }
    }

    public override void OnClickSlot(GameObject go, PointerEventData data)
    {
        // quickslot does nothing when cilcked
    }
}
