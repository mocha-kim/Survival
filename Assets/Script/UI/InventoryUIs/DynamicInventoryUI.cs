using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField]
    protected GameObject slotParent;
    [SerializeField]
    protected GameObject slotPrefab;
    protected float slotSize;

    [SerializeField]
    protected Vector2 start;
    [SerializeField]
    protected Vector2 space;
    [SerializeField]
    protected int colNum;

    protected override void CreateSlots()
    {
        slotSize = slotPrefab.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent.transform);
            newSlot.name += " " + i;

            float x = start.x + ((space.x + slotSize) * (i % colNum));
            float y = start.y + (-(space.y + slotSize) * (i / colNum));
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
}
