using UnityEngine;

/*
 * NPCScript is players and NPCs interact trigger
 * This helps the player to talk to NPCs
 * 
 * This manages NPC dialogue in the inspector
 * https://www.youtube.com/watch?v=p4a_OYmk1uU
 */

public class NPCDialogue : MonoBehaviour
{
    // Component
    public GameObject Dialogue;
    public GameObject NPCCharacter;
    private NPCDialogue npcDialogue;
    private QuestDialogue questDialogue;

    // NPC Name and Dialogue
    [SerializeField]
    private NPCDatabase database;
    [SerializeField]
    private int npcID;
    [TextArea(5, 10)]
    public string[] sentences;

    [SerializeField]
    private float distance = 10;
    public float Distance => distance;
    private bool isPlayerEnter = false;

    void Start()
    {
        npcDialogue = GetComponent<NPCDialogue>();
        questDialogue = GetComponent<QuestDialogue>();
        NPCCharacter = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerEnter = true;
            npcDialogue.enabled = true;
            DialogueManager.Instance.EnterRangeOfNPC();
        }
    }

    private void OnTriggerExit()
    {
        isPlayerEnter = false;
        DialogueManager.Instance.OutOfRange();
        npcDialogue.enabled = false;
    }

    private void Update()
    {
        if (isPlayerEnter)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (questDialogue == null)
            {
                npcDialogue.enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    npcDialogue.enabled = true;
                    DialogueManager.Instance.names = database.datas[npcID].name;
                    DialogueManager.Instance.StartDialogue(in sentences);
                }
            }
            else if (!questDialogue.isStartQuestDialogue && questDialogue.questObject.status == QuestStatus.Rewarded)
            {
                npcDialogue.enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    npcDialogue.enabled = true;
                    DialogueManager.Instance.names = database.datas[npcID].name;
                    DialogueManager.Instance.StartDialogue(in sentences);
                }
            }
        }
    }
}