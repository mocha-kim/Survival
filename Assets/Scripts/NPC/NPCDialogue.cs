using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * NPCScript is players and NPCs interact trigger
 * This helps the player to talk to NPCs
 * 
 * This manages NPC dialogue in the inspector
 * https://www.youtube.com/watch?v=p4a_OYmk1uU
 */

public class NPCDialogue : MonoBehaviour, IInteractable
{
    // Component
    public GameObject Dialogue;
    public GameObject NPCCharacter;
    private NPCDialogue npcDialogue;
    private QuestDialogue questDialogue;

    // NPC Name and Dialogue
    public string NPCName;
    [TextArea(5, 10)]
    public string[] sentences;

    [SerializeField]
    private float distance = 10;
    public float Distance => distance;

    void Start()
    {
        npcDialogue = GetComponent<NPCDialogue>();
        questDialogue = GetComponent<QuestDialogue>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (questDialogue == null)
        {
            npcDialogue.enabled = true;
            DialogueManager.Instance.EnterRangeOfNPC();
            if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                npcDialogue.enabled = true;
                DialogueManager.Instance.names = NPCName;
                DialogueManager.Instance.StartDialogue(in sentences);
            }
        }
        else if (!questDialogue.isStartQuestDialogue && questDialogue.questObject.status == QuestStatus.Rewarded)
        {
            npcDialogue.enabled = true;
            DialogueManager.Instance.EnterRangeOfNPC();
            if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                npcDialogue.enabled = true;
                DialogueManager.Instance.names = NPCName;
                DialogueManager.Instance.StartDialogue(in sentences);
            }
        }
    }

    public void OnTriggerExit()
    {
        DialogueManager.Instance.OutOfRange();
        npcDialogue.enabled = false;
    }

    public void Interact(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void StopInteract(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}