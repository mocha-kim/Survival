using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Database", menuName = "Database/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public Enemy[] datas;

    public void OnValidate()
    {
        for (int i = 0; i < datas.Length; ++i)
        {
            datas[i].id = i;
        }
    }
}
