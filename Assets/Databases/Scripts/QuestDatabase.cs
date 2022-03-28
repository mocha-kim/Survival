using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Database", menuName = "Database/Quest Database")]
public class QuestDatabase : ScriptableObject
{
    public List<QuestObject> questObjects;
    public int Count => questObjects.Count;

    public void OnValidate()
    {
        for (int i = 0; i < questObjects.Count; ++i)
            questObjects[i].data.id = i;
    }

    public void Add(QuestObject obj)
    {
        questObjects.Add(obj);
    }

    public void Remove(QuestObject obj)
    {
        questObjects.Remove(obj);
    }

    public int IndexOf(QuestObject obj)
    {
        return questObjects.IndexOf(obj);
    }
}
