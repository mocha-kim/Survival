using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class QuestSlotUI : MonoBehaviour
{
    public QuestObject quest;

    [SerializeField]
    private TextMeshProUGUI[] texts;
    [SerializeField]
    private Sprite[] backgrounds;

    private void Awake()
    {
        QuestManager.Instance.OnUpdateQuest += OnUpdateQuest;
    }

    public void UpdateTexts()
    {
        texts[0].text = quest.title;
        if (quest.type == QuestType.AcquireItem)
        {
            texts[1].text = GameManager.Instance.GetItemName(quest.data.targetID);
        }
        else
        {
            texts[1].text = GameManager.Instance.GetEnemyName(quest.data.targetID);
        }
        texts[2].text = quest.data.currentCount + "/" + quest.data.goalCount;
    }

    public void UpdateBackground()
    {
        switch (quest.status)
        {
            case QuestStatus.Accepted:
                GetComponent<Image>().sprite = backgrounds[0];
                break;
            case QuestStatus.Completed:
                GetComponent<Image>().sprite = backgrounds[1];
                break;
            case QuestStatus.Rewarded:
                GetComponent<Image>().sprite = backgrounds[2];
                break;
            default:
                break;
        }
    }

    public void OnUpdateQuest(QuestObject quest)
    {
        if (this.quest.data.id == quest.data.id)
        {
            UpdateTexts();
        }
    }

    public void OnRewardedQuest(int index)
    {
        QuestManager.Instance.OnUpdateQuest -= OnUpdateQuest;
    }
}
