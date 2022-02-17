using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/New Inventory")]
public class InventoryObject : ScriptableObject
{
    public InventoryType type;
    public ItemDatabase database;
    [SerializeField]
    private Inventory data = new(24);

    public Action<ItemObject> OnUseItem;

    public InventorySlot[] Slots => data.slots;

    public int EmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (InventorySlot slot in Slots)
            {
                if (slot.item.id == -1)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public InventorySlot GetItemSlot(Item item)
    {
        return Slots.FirstOrDefault(i => i.item.id == item.id);
    }

    public InventorySlot GetEmptySlot()
    {
        return Slots.FirstOrDefault(i => i.item.id == -1);
    }

    public bool AddItem(Item item, int amount)
    {
        InventorySlot slotToAdd = GetItemSlot(item);

        if (!database.itemObjects[item.id].isStackable || slotToAdd == null)
        {
            if (EmptySlotCount <= 0)
            {
                return false;
            }
            GetEmptySlot().AddItem(item, amount);
        }
        else
        {
            slotToAdd.AddItem(item, amount);
        }

        return true;
    }

    public bool IsContain(ItemObject itemObject)
    {
        return Slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
    }

    public void SwapItems(InventorySlot slotA, InventorySlot slotB)
    {
        if (slotA == slotB)
        {
            return;
        }

        if (slotA.AllowedToPlace(slotB.ItemObject) && slotB.AllowedToPlace(slotA.ItemObject))
        {
            InventorySlot tmpSlot = new InventorySlot(slotB.item, slotB.amount);
            slotB.UpdateSlot(slotA.item, slotA.amount);
            slotA.UpdateSlot(tmpSlot.item, tmpSlot.amount);
        }
    }

    public void UseItem(InventorySlot slot)
    {
        if (slot.ItemObject == null || slot.item.id < 0 || slot.amount <= 0)
            return;

        ItemObject itemObject = slot.ItemObject;
        slot.UpdateSlot(slot.item, slot.amount - 1);

        OnUseItem.Invoke(itemObject);
    }
}
