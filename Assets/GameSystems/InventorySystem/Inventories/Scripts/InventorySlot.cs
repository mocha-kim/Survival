using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedType;
    public Item item;
    public int amount;

    [NonSerialized]
    public InventoryObject parent;
    [NonSerialized]
    public GameObject slotUI;

    public Action<InventorySlot> OnPreUpdate;
    public Action<InventorySlot> OnPostUpdate;

    public ItemObject ItemObject
    {
        get
        {
            return item.id >= 0 ? parent.database.itemObjects[item.id] : null;
        }
    }

    public InventorySlot()
    {
        UpdateSlot(item, amount);
    }

    public InventorySlot(Item item, int amount)
    {
        UpdateSlot(item, amount);
    }

    public void UpdateSlot(Item item, int amount)
    {
        if (amount <= 0)
        {
            item = new();
        }

        OnPreUpdate?.Invoke(this);

        this.item = item;
        this.amount = amount;

        OnPostUpdate?.Invoke(this);
    }

    public void AddItem(Item item, int amount)
    {
        UpdateSlot(item, amount);
    }

    public void RemoveItem()
    {
        QuestManager.Instance.ProcessQuest(QuestType.AcquireItem, item.id, -amount);
        UpdateSlot(null, 0);
    }

    public void AddItemAmount(int amount)
    {
        UpdateSlot(item, this.amount += amount);
    }

    public bool AllowedToPlace(ItemObject itemObject)
    {
        if (allowedType.Length <= 0 || itemObject == null || itemObject.data.id < 0)
        {
            return true;
        }

        foreach (ItemType type in allowedType)
        {
            if (itemObject.type == type)
                return true;
        }

        return false;
    }
}