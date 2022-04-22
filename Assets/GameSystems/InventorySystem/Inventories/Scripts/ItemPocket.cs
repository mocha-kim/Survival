using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPocket : MonoBehaviour
{
    // Component
    public EnemyDatabase database;
    public ItemDatabase itemDatabase;
    public InventoryObject inventoryObject;

    public int pocketID = -1;
    public float distance;
    private int itemID;
    private int itemCount;

    public void Interact(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void StopInteract(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void GenEnemyItem(int enemyID)
    {
        Enemy enemy = database.data[enemyID];
        GenItemPocket(enemy);
    }

    public void GenItemPocket(Enemy enemy)
    {
        pocketID++;
        for (int i = 0; i < enemy.rewardItem.Count; i++)
        {
            itemCount = 0;
            itemID = enemy.rewardItem[i].item.data.id;

            for (int j = 0; j < enemy.rewardItem[i].count; j++)
            {
                if (Random.Range(0, 100) <= enemy.rewardItem[i].percentage)
                {
                    itemCount++;
                }
            }
            if(itemCount > 0)
            {
                inventoryObject.Slots[i].UpdateSlot(itemDatabase.GetItem(itemID), itemCount);
                //genPockets[pocketID].Slots[i].UpdateSlot(itemDatabase.GetItem(itemID), itemCount);
            }
        }
    }
}
