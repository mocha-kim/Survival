using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PocketData
{
    public Enemy enemy;
    public EnemyDatabase database;
    public ItemDatabase itemDatabase;

    public int pocketID = 0;
    private int itemID;
    private int itemCount;

    public List<InventoryObject> genItem = new();
    
    public void GenItems(int enemyID)
    {
        Debug.Log("AAA");
        Debug.Log(database.data[enemyID]);
        Debug.Log(enemy);
        Debug.Log("AAAB");

        enemy = database.data[enemyID];
        Debug.Log("AAB");

        for (int i = 0; i < enemy.rewardItem.Count; i++)
        {
            itemCount = 0;
            itemID = enemy.rewardItem[i].item.data.id;

            for(int j = 0; j < enemy.rewardItem[i].count; j++)
            {
                if (Random.Range(0, 100) <= enemy.rewardItem[i].percentage)
                {
                    itemCount++;
                }
            }
            genItem[i].AddItem(itemDatabase.GetItem(itemID), itemCount);
        }
        pocketID++;
    }
}
