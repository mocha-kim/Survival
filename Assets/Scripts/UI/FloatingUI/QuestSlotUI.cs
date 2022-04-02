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
        QuestManager.Instance.OnUpdateQuestStatus += OnUpdateQuestStatus;
        QuestManager.Instance.OnRewardedQuest += OnRewardedQuest;
    }

    public void UpdateTexts()
    {
        texts[0].text = quest.title;
        if (quest.type == QuestType.DestoryEnemy)
        {
            texts[1].text = GameManager.Instance.GetEnemyName(quest.data.targetID);
        }
        else
        {
            texts[1].text = GameManager.Instance.GetItemName(quest.data.targetID);
        }
        UpdateCountText();
    }

    public void UpdateCountText()
    {
        if (quest.data.currentCount >= quest.data.goalCount)
        {
            texts[2].text = "완료";
        }
        else
        {
            texts[2].text = quest.data.currentCount + "/" + quest.data.goalCount;
        }
    }

    public void UpdateSlotStyle()
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
                texts[0].color = Color.grey;
                texts[1].color = Color.grey;
                texts[2].color = Color.grey;
                break;
            default:
                break;
        }
    }

    public void OnUpdateQuest(QuestObject quest)
    {
        if (this.quest.data.id == quest.data.id)
        {
            UpdateCountText();
        }
    }

    public void OnUpdateQuestStatus(QuestObject quest, bool flag)
    {
        UpdateSlotStyle();
    }

    public void OnRewardedQuest(QuestObject quest)
    {
        UpdateSlotStyle();
        QuestManager.Instance.OnUpdateQuest -= OnUpdateQuest;
    }
}
