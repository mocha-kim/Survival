using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemPocketGenerator : MonoBehaviour
{
    private static ItemPocketGenerator instance;
    public static ItemPocketGenerator Instance => instance;

    [SerializeField]
    private ItemDatabase database;
    [SerializeField]
    private GameObject itemPocketPrefab;

    private const string path = "Assets/GameSystems/InventorySystem/Inventories/";
    private int pocketCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);   
    }

    public void GenItemPocket(Vector3 enemyPosition, Enemy enemy)
    {
        pocketCount++;

        InventoryObject inventoryObject = ScriptableObject.CreateInstance<InventoryObject>();
        if (inventoryObject == null)
        {
            Debug.Log("Inventory Object did not created");
            return;
        }

        inventoryObject.ResizeSlotSize(8);
        inventoryObject.type = InventoryType.Chest;
        inventoryObject.database = database;

        AssetDatabase.CreateAsset(inventoryObject, path + "ItemPocket " + pocketCount + ".asset");
        InventoryObject itemPocketSO = AssetDatabase.LoadAssetAtPath<InventoryObject>(path + "ItemPocket " + pocketCount + ".asset");
        if (itemPocketSO == null)
        {
            Debug.Log("ItemPocket SO did not created");
            return;
        }

        int itemID, itemCount;
        for (int i = 0; i < enemy.rewardItem.Count; i++)
        {
            itemID = enemy.rewardItem[i].item.data.id;
            if (Random.Range(0, 100) <= enemy.rewardItem[i].percentage)
            {
                itemCount = Random.Range(1, enemy.rewardItem[i].count);
                inventoryObject.Slots[i].UpdateSlot(inventoryObject.database.GetItem(itemID), itemCount);
            }
        }

        GameObject itemPocketGO = Instantiate(itemPocketPrefab);
        itemPocketGO.transform.position = enemyPosition;
        itemPocketGO.name = "ItemPocket " + pocketCount;

        ItemPocket itemPocket = itemPocketGO.GetComponent<ItemPocket>();
        itemPocket.SetInventoryObject(itemPocketSO);
    }

    public void DeleteItemPocket(InventoryObject inventoryObject)
    {
        AssetDatabase.DeleteAsset(path + inventoryObject.name + ".asset");
    }

}
