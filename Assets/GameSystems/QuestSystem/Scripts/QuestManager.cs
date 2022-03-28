using System;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance => instance;

    public QuestDatabase questDatabase;

    public QuestDatabase acceptedQuests;
    public QuestDatabase rewardedQuests;

    public event Action<QuestObject, bool> OnUpdateQuestStatus;
    public event Action<QuestObject> OnUpdateQuest;
    public event Action<QuestObject> OnRewardedQuest;

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

    // This method dose not guarantee the correct step of status and just changes status of quest
    public void UpdateQuestStatus(QuestObject quest, QuestStatus status)
    {
        bool createNewItemFlag = false;

        switch (status)
        {
            case QuestStatus.None:
            case QuestStatus.Accepted:
                if (IsUnique(quest) && quest.status == QuestStatus.None)
                {
                    createNewItemFlag = true;
                    acceptedQuests.Add(quest);
                }
                break;
            case QuestStatus.Rewarded:
                RewardQuest(quest);
                break;
        }
        quest.status = status;

        OnUpdateQuestStatus?.Invoke(quest, createNewItemFlag);
    }

    public void ProcessQuest(QuestType type, int targetId, int value)
    {
        foreach (QuestObject quest in acceptedQuests.questObjects)
        {
            if ((quest.data.targetID == targetId) && (quest.type == type))
            {
                quest.data.currentCount += value;
                if (quest.data.currentCount == quest.data.goalCount)
                {
                    UpdateQuestStatus(quest, QuestStatus.Completed);
                }
                else if (value < 0 && (quest.data.currentCount < quest.data.goalCount))
                {
                    UpdateQuestStatus(quest, QuestStatus.Accepted);
                }
                OnUpdateQuest?.Invoke(quest);
            }
        }
    }

    public void SetQuestCurValue(QuestObject quest, int value)
    {
        quest.data.currentCount = value;
        if (quest.data.currentCount == quest.data.goalCount)
        {
            UpdateQuestStatus(quest, QuestStatus.Completed);
        }
        else if (quest.data.currentCount < quest.data.goalCount)
        {
            UpdateQuestStatus(quest, QuestStatus.Accepted);
        }
        OnUpdateQuest?.Invoke(quest);
    }

    private void RewardQuest(QuestObject quest)
    {
        //if (quest.status != QuestStatus.Completed)
        //{
        //    Debug.Log("This quest(" + quest + ") didn't completed");
        //    return;
        //}
        if (rewardedQuests.questObjects.FirstOrDefault(i => i == quest))
        {
            return;
        }
        // add exp or items to player
        OnRewardedQuest?.Invoke(quest);

        acceptedQuests.Remove(quest);
        rewardedQuests.Add(quest);
    }

    private bool IsUnique(QuestObject quest)
    {
        if (acceptedQuests.questObjects.FirstOrDefault(i => i == quest)
            || rewardedQuests.questObjects.FirstOrDefault(i => i == quest))
        {
            return false;
        }
        return true;
    }
}
