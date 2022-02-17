using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public InventorySlot[] slots;

    public Inventory(int capacity)
    {
        slots = new InventorySlot[capacity];
    }

    public void Clear()
    {
        foreach(InventorySlot slot in slots)
        {
            slot.RemoveItem();
        }
    }

    public bool IsContain(ItemObject itemObject)
    {
        return IsContain(itemObject.data.id);
    }

    public bool IsContain(int id)
    {
        return slots.FirstOrDefault(i => i.item.id == id) != null;
    }
}