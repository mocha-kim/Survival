using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
