using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticUI : InventoryUI
{
    public GameObject[] staticSlots = null;

    protected override void CreateSlots()
    {
        Debug.Log("static");
        slots = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            GameObject slotUI = staticSlots[i];

            AddEvent(slotUI, EventTriggerType.PointerEnter, delegate { OnEnterSlot(slotUI); });
            AddEvent(slotUI, EventTriggerType.PointerExit, delegate { OnExitSlot(slotUI); });
            AddEvent(slotUI, EventTriggerType.BeginDrag, delegate { OnStartDrag(slotUI); });
            AddEvent(slotUI, EventTriggerType.Drag, delegate { OnDrag(slotUI); });
            AddEvent(slotUI, EventTriggerType.EndDrag, delegate { OnEndDrag(slotUI); });
            AddEvent(slotUI, EventTriggerType.PointerClick, (data) => { OnClick(slotUI, (PointerEventData)data); });

            inventoryObject.Slots[i].slotUI = slotUI;
            inventoryObject.Slots[i].parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;

            slots.Add(slotUI, inventoryObject.Slots[i]);
        }
    }
}
