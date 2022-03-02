using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/New Item Database")]
public class ItemDatabase : ScriptableObject
{
    public ItemObject[] itemObjects;

    public void OnValidate()
    {
        for (int i = 0; i < itemObjects.Length; ++i)
        {
            itemObjects[i].data.id = i;
        }
    }

    public Item GetItem(int id)
    {
        return itemObjects.FirstOrDefault<ItemObject>(i => i.data.id == id).data;
    }

    public Item GetItem(string name)
    {
        return itemObjects.FirstOrDefault<ItemObject>(i => i.data.name == name).data;
    }
}
