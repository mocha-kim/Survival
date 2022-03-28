using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Database", menuName = "Database/NPC Database")]
public class NPCDatabase : ScriptableObject
{
    public NPC[] datas;

    public void OnValidate()
    {
        for (int i = 0; i < datas.Length; ++i)
        {
            datas[i].id = i;
        }
    }
}
