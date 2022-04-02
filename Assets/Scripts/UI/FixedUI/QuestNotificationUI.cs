using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestNotificationUI : MonoBehaviour
{
    [SerializeField]
    private GameObject questNotiSlotPrefab;

    [SerializeField]
    private Sprite[] backgrounds;

    public float destroyDelay = 1f;

    private float slotHeight = 45f;
    private float space = 10f;
    private int slotCount = 0;

    private void Awake()
    {
        QuestManager.Instance.OnUpdateQuestStatus += OnUpdateQuestStatus;
    }

    public void OnUpdateQuestStatus(QuestObject quest, bool createNewItemFlag)
    {
        if ((quest.status == QuestStatus.Accepted && createNewItemFlag) || quest.status == QuestStatus.Completed)
        {
            GameObject go = Instantiate(questNotiSlotPrefab, transform);
            go.name += " " + slotCount;

            float y = -(space + slotHeight) * slotCount;
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, y);
            go.GetComponent<Image>().sprite = (quest.status == QuestStatus.Accepted) ? backgrounds[0] : backgrounds[1];

            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = quest.title;
            texts[1].text = quest.summary;
            texts[2].text = (quest.status == QuestStatus.Accepted) ? quest.data.currentCount + "/" + quest.data.goalCount : "완료";

            slotCount++;
            StartCoroutine(QuestNotify(go));
        }
    }

    IEnumerator QuestNotify(GameObject go)
    {
        Image image = go.GetComponent<Image>();
        for (float f = 0f; f < 1f; f += 0.02f)
        {
            Color c = image.color;
            c.a = f;
            image.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(destroyDelay);
        for (float f = 1f; f > 0f; f -= 0.02f)
        {
            Color c = image.color;
            c.a = f;
            image.color = c;
            yield return null;
        }
        Destroy(go);
        slotCount--;
    }
}
