using System;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance => instance;

    public QuestDatabase questDatabase;
    public QuestDataContainer playerQuests;

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
        foreach (QuestObject quest in playerQuests.acceptedQuests)
        {
            if (quest.type == QuestType.AcquireItem)
            {
                SetCurrentValue(quest, GameManager.Instance.GetTotalItemCount(quest.data.targetID));
            }
        }
    }

    public void ProcessQuest(QuestType type, int targetId, int value)
    {
        foreach (QuestObject quest in playerQuests.acceptedQuests)
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

    // This method dose not guarantee the correct step of status. This just changes status of quest
    public bool UpdateQuestStatus(QuestObject quest, QuestStatus status)
    {
        bool rewardResult = true;
        if (quest.status == status) return false;
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
                rewardResult = RewardQuest(quest);
                break;
        }
        return rewardResult;
    }

    private void AccepteQuest(QuestObject quest)
    {
        bool createNewItemFlag = false;

        if (IsUnique(quest) && quest.status == QuestStatus.None)
        {
            createNewItemFlag = true;
            playerQuests.acceptedQuests.Add(quest);
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

    private bool RewardQuest(QuestObject quest)
    {
        if (quest.status != QuestStatus.Completed)
        {
            Debug.Log("This quest(" + quest + ") didn't completed");
            return false;
        }
        if (playerQuests.rewardedQuests.FirstOrDefault(i => i == quest))
        {
            Debug.Log("This quest(" + quest + ") already rewarded");
            return false;
        }

        // add gold or items to player
        InventoryObject inventory = GameManager.Instance.inventory;
        int requiredSlotCount = 0;
        for (int i = 0; i < quest.data.rewardItemIds.Count; i++)
        {
            if (GameManager.Instance.IsItemStackable(quest.data.rewardItemIds[i]))
            {
                if (!inventory.IsContain(quest.data.rewardItemIds[i]))
                {
                    requiredSlotCount++;
                }
            }
            else
            {
                requiredSlotCount++;
            }
        }
        if (inventory.EmptySlotCount < requiredSlotCount)
        {
            Debug.Log("Player does not have enough inventory space");
            return false;
        }

        for (int i = 0; i < quest.data.rewardItemIds.Count; i++)
        {
            inventory.AddItem(quest.data.rewardItemIds[i], quest.data.rewardItemCounts[i]);
        }

        playerQuests.acceptedQuests.Remove(quest);
        playerQuests.rewardedQuests.Add(quest);

        quest.status = QuestStatus.Rewarded;

        OnRewardedQuest?.Invoke(quest);
        return true;
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
        if (playerQuests.acceptedQuests.FirstOrDefault(i => i == quest)
            || playerQuests.rewardedQuests.FirstOrDefault(i => i == quest))
        {
            return false;
        }
        return true;
    }
}
