using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pocket Database", menuName = "Database/PocketDatabase")]
public class ItemPocketDatabase : ScriptableObject
{
    public List<ItemPocket> data;

    public void OnValidate()
    {
        for (int i = 0; i < data.Count; ++i)
        {
            data[i].pocketID = i;
        }
    }
}
