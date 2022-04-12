using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Database", menuName = "Database/Quest Database")]
public class QuestDatabase : ScriptableObject
{
    public List<QuestObject> data;

    public void OnValidate()
    {
        for (int i = 0; i < data.Count; ++i)
            data[i].data.id = i;
    }
}
