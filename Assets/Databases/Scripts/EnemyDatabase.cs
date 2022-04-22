using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Database", menuName = "Database/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public List<Enemy> data;

    public void OnValidate()
    {
        for (int i = 0; i < data.Count; ++i)
        {
            data[i].id = i;
        }
    }
}
