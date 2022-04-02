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

    private void Start()
    {
        foreach (QuestObject quest in acceptedQuests.questObjects)
        {
            if (quest.type == QuestType.AcquireItem)
            {
                SetCurrentValue(quest, GameManager.Instance.GetTotalItemCount(quest.data.targetID));
            }
        }
    }

    public void ProcessQuest(QuestType type, int targetId, int value)
    {
        foreach (QuestObject quest in acceptedQuests.questObjects)
        {
            if ((quest.data.targetID == targetId) && (quest.type == type))
            {
                quest.data.currentCount += value;
                if (quest.data.currentCount < 0)
                {
                    quest.data.currentCount = 0;
                }
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

    // This method dose not guarantee the correct step of status and just changes status of quest
    public void UpdateQuestStatus(QuestObject quest, QuestStatus status)
    {
        switch (status)
        {
            case QuestStatus.None:
                break;
            case QuestStatus.Accepted:
                AccepteQuest(quest);
                break;
            case QuestStatus.Completed:
                CompleteQuest(quest);
                break;
            case QuestStatus.Rewarded:
                RewardQuest(quest);
                break;
        }
    }

    private void AccepteQuest(QuestObject quest)
    {
        bool createNewItemFlag = false;

        if (IsUnique(quest) && quest.status == QuestStatus.None)
        {
            createNewItemFlag = true;
            acceptedQuests.Add(quest);
        }

        quest.status = QuestStatus.Accepted;
        OnUpdateQuestStatus?.Invoke(quest, createNewItemFlag);

        if (quest.data.currentCount >= quest.data.goalCount)
        {
            CompleteQuest(quest);
        }
    }

    private void CompleteQuest(QuestObject quest)
    {
        quest.status = QuestStatus.Completed;
        OnUpdateQuestStatus?.Invoke(quest, false);
    }

    private void RewardQuest(QuestObject quest)
    {
        if (quest.status != QuestStatus.Completed)
        {
            Debug.Log("This quest(" + quest + ") didn't completed");
            return;
        }
        if (rewardedQuests.questObjects.FirstOrDefault(i => i == quest)) return;
        // add gold or items to player

        acceptedQuests.Remove(quest);
        rewardedQuests.Add(quest);

        quest.status = QuestStatus.Rewarded;

        OnRewardedQuest?.Invoke(quest);
    }

    public void SetCurrentValue(QuestObject quest, int value)
    {
        quest.data.currentCount = value;
        if (quest.data.currentCount >= quest.data.goalCount)
        {
            UpdateQuestStatus(quest, QuestStatus.Completed);
        }
        else if (quest.data.currentCount < quest.data.goalCount)
        {
            UpdateQuestStatus(quest, QuestStatus.Accepted);
        }
        
        OnUpdateQuest?.Invoke(quest);
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
