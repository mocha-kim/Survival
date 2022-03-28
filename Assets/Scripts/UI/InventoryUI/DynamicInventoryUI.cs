using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [SerializeField]
    protected Sprite[] slotBackgounds;
    protected GameObject previousSlot;

    protected override void CreateSlots()
    {
        slotSize = slotPrefab.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent.transform);
            newSlot.name += " " + i;

            float x = start.x + ((space.x + slotSize) * (i % colNum));
            float y = start.y + (-(space.y + slotSize) * (i / colNum));
            newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            // slot related events
            AddEvent(newSlot, EventTriggerType.PointerEnter, delegate { OnEnterSlot(newSlot); });
            AddEvent(newSlot, EventTriggerType.PointerExit, delegate { OnExitSlot(newSlot); });
            AddEvent(newSlot, EventTriggerType.BeginDrag, delegate { OnStartDragItem(newSlot); });
            AddEvent(newSlot, EventTriggerType.Drag, delegate { OnDragItem(newSlot); });
            AddEvent(newSlot, EventTriggerType.EndDrag, delegate { OnEndDragItem(newSlot); });
            AddEvent(newSlot, EventTriggerType.PointerClick, (data) => { OnClickSlot(newSlot, (PointerEventData)data); });

            inventoryObject.Slots[i].slotUI = newSlot;
            inventoryObject.Slots[i].parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;

            slots.Add(newSlot, inventoryObject.Slots[i]);
        }
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
