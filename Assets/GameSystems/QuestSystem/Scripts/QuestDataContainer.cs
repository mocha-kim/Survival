using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Quest System/Quest Data Container")]
public class QuestDataContainer : ScriptableObject
{
    public List<QuestObject> acceptedQuests;
    public List<QuestObject> rewardedQuests;

    public void ResetQuests()
    {
        foreach (QuestObject quest in acceptedQuests)
        {
            quest.ResetQuestProcess();
        }
        acceptedQuests.Clear();

        foreach (QuestObject quest in rewardedQuests)
        {
            quest.ResetQuestProcess();
        }
        rewardedQuests.Clear();
    }
}
