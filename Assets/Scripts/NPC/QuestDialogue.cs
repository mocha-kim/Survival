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
    [SerializeField]
    private NPCDatabase database;
    [SerializeField]
    private int npcID;
    [TextArea(5, 10)]
    public string[] readySentences;
    [TextArea(5, 10)]
    public string[] acceptedSentences;
    [TextArea(5, 10)]
    public string[] completedSentences;
    public bool isStartQuestDialogue = false;
    private bool isPlayerEnter = false;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerEnter = true;
            questDialogue.enabled = true;
            DialogueManager.Instance.quest = questObject;
        }
    }

    private void OnTriggerExit()
    {
        isPlayerEnter = false;
        CheckQuestGO();
        DialogueManager.Instance.OutOfRange();
        DialogueManager.Instance.quest = null;
        questDialogue.enabled = false;
        isStartQuestDialogue = false;
    }

    private void Update()
    {
        CheckQuestGO();
        if (isPlayerEnter)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isStartQuestDialogue)
                {
                    return;
                }

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                questDialogue.enabled = true;
                isStartQuestDialogue = true;
                DialogueManager.Instance.EnterRangeOfNPC();
                DialogueManager.Instance.names = database.datas[npcID].name;

                if (questObject.status == QuestStatus.None)
                {
                    DialogueManager.Instance.StartDialogue(in readySentences);
                }
                else if (questObject.status == QuestStatus.Accepted)
                {
                    DialogueManager.Instance.StartDialogue(in acceptedSentences);
                }
                else if (questObject.status == QuestStatus.Completed)
                {
                    // Reward quest
                    QuestManager.Instance.UpdateQuestStatus(questObject, QuestStatus.Rewarded);
                    DialogueManager.Instance.StartDialogue(in completedSentences);
                }
                else if (questObject.status == QuestStatus.Rewarded)
                {
                    isStartQuestDialogue = false;
                }
            }
            isStartQuestDialogue = false;
            CheckAccept();
        }
    }

    public void CheckAccept()
    {
        if (DialogueManager.Instance.isaccept == true)
        {
            QuestManager.Instance.UpdateQuestStatus(questObject, QuestStatus.Accepted);
            DialogueManager.Instance.isaccept = false;
        }
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
}
