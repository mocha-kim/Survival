using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public InventoryType type;
    public ItemDatabase database;
    [SerializeField]
    private Inventory data = new(24);
    public Inventory ItemPocketSlot = new(8);

    public InventorySlot[] Slots => data.slots;

    public Action<ItemObject> OnUseItem;

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

    public InventorySlot GetItemSlot(Item item) => Slots.FirstOrDefault(i => i.item.id == item.id);
    public InventorySlot GetItemSlot(int id) => Slots.FirstOrDefault(i => i.item.id == id);
    public InventorySlot GetEmptySlot() => Slots.FirstOrDefault(i => i.item.id == -1);

    public void ResizeSlotSize(int size)
    {
        Array.Resize(ref data.slots, size);
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
            slotToAdd.AddItemAmount(amount);
        }

        QuestManager.Instance.ProcessQuest(QuestType.AcquireItem, item.id, amount);
        return true;
    }

    public bool AddItem(int id, int amount)
    {
        Item item = database.itemObjects.FirstOrDefault(i => i.data.id == id).data;
        return AddItem(item, amount);
    }

    public bool IsContain(ItemObject itemObject) => Slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
    public bool IsContain(int id) => Slots.FirstOrDefault(i => i.item.id == id) != null;

    public int CountItem(int id)
    {
        int count = 0;
        foreach (InventorySlot slot in Slots)
        {
            if (slot.item.id == id)
            {
                count += slot.amount;
            }
        }
        return count;
    }

    public void SwapItems(InventorySlot slotA, InventorySlot slotB)
    {
        Debug.Log("Swap items : " + slotA.ItemObject + ", " + slotB.ItemObject);
        if (slotA == slotB)
        {
            return;
        }

        if (slotA.AllowedToPlace(slotB.ItemObject) && slotB.AllowedToPlace(slotA.ItemObject))
        {
            if ((slotA.item.id == slotB.item.id) && slotA.ItemObject.isStackable)
            {
                int value = slotA.amount;
                slotA.RemoveItem();
                slotB.AddItemAmount(value);
            }
            else
            {
                InventorySlot tmpSlot = new InventorySlot(slotB.item, slotB.amount);
                slotB.UpdateSlot(slotA.item, slotA.amount);
                slotA.UpdateSlot(tmpSlot.item, tmpSlot.amount);
            }
        }
    }

    public void ClearInventory()
    {
        foreach (InventorySlot slot in Slots)
        {
            slot.UpdateSlot(null, 0);
        }
    }

    public void UseItem(InventorySlot slot)
    {
        if (slot.ItemObject == null || slot.item.id < 0 || slot.amount <= 0)
        {
            return;
        }

        QuestManager.Instance.ProcessQuest(QuestType.AcquireItem, slot.item.id, -1);

        OnUseItem?.Invoke(slot.ItemObject);
        slot.UpdateSlot(slot.item, slot.amount - 1);
    }
}
