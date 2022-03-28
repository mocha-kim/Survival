using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDialogue : MonoBehaviour
{
    // Component
    public GameObject Dialogue;
    public GameObject NPCCharacter;
    public QuestObject questObject;
    private QuestDialogue questDialogue;

    public GameObject questEffectGO;   // [!]
    public GameObject questAcceptedGO; // [...]
    public GameObject questRewardGO;   // [?]

    // NPC Name and Dialogue
    public string NPCName;
    [TextArea(5, 10)]
    public string[] readySentences;
    [TextArea(5, 10)]
    public string[] acceptedSentences;
    [TextArea(5, 10)]
    public string[] completedSentences;
    public bool isStartQuestDialogue = false;

    public float distance = 2.0f;
    public float Distance => distance;

    void Start()
    {
        questDialogue = GetComponent<QuestDialogue>();

        questEffectGO.SetActive(false);
        questRewardGO.SetActive(false);
        questAcceptedGO.SetActive(false);
        CheckQuestGO();
    }

    public void OnTriggerStay(Collider other)
    {
        CheckQuestGO();
        questDialogue.enabled = true;
        DialogueManager.Instance.EnterRangeOfNPC();
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            if (isStartQuestDialogue)
            {
                return;
            }

            questDialogue.enabled = true;
            isStartQuestDialogue = true;
            DialogueManager.Instance.names = NPCName;

            if (questObject.status == QuestStatus.None)
            {
                DialogueManager.Instance.StartDialogue(in readySentences);
                QuestManager.Instance.UpdateQuestStatus(questObject, QuestStatus.Accepted);
            }
            else if (questObject.status == QuestStatus.Accepted)
            {
                DialogueManager.Instance.StartDialogue(in acceptedSentences);
            }
            else if (questObject.status == QuestStatus.Completed)
            {
                // Reward quest
                DialogueManager.Instance.StartDialogue(in completedSentences);
                QuestManager.Instance.UpdateQuestStatus(questObject, QuestStatus.Rewarded);
            }
            else if (questObject.status == QuestStatus.Rewarded)
            {
                isStartQuestDialogue = false;
            }
        }
    }

    public void OnTriggerExit()
    {
        CheckQuestGO();
        DialogueManager.Instance.OutOfRange();
        questDialogue.enabled = false;
        isStartQuestDialogue = false;
    }

    private void CheckQuestGO()
    {
        if (questObject.status == QuestStatus.None)
        {
            questEffectGO.SetActive(true);
        }
        else if (questObject.status == QuestStatus.Accepted)
        {
            questEffectGO.SetActive(false);
            questAcceptedGO.SetActive(true);
        }
        else if (questObject.status == QuestStatus.Completed)
        {
            questEffectGO.SetActive(false);
            questAcceptedGO.SetActive(false);
            questRewardGO.SetActive(true);
        }
        else
        {
            questEffectGO.SetActive(false);
            questRewardGO.SetActive(false);
            questAcceptedGO.SetActive(false);
        }
    }

    private void OnCompletedQuest(QuestObject questObject)
    {
        if (questObject.data.id == this.questObject.data.id && questObject.status == QuestStatus.Completed)
        {
            questEffectGO.SetActive(false);
            questRewardGO.SetActive(true);
        }
    }
}
