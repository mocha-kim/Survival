using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * DialogManager is players and NPCs interact manager
 * This helps the player to talk to NPCs
 * 
 * This manages NPC dialogue in the inspector
 * https://www.youtube.com/watch?v=p4a_OYmk1uU
 * http://nickithansen.dk/waitforseconds-and-time-timescale-0/
 */

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance => instance;

    // Component
    public GameObject toChat;     // [F] To Chat
    public GameObject dialogueUI; // Dialogue
    public GameObject YesButton;  // Yes Button
    public GameObject NoButton;   // No Button
    public Text nameText;         // NPC Name
    public Text dialogueText;     // NPC Dialogue
    public QuestObject quest;     // Quest

    // Setting
    [SerializeField]
    private float readSpeed = 0.05f;
    [SerializeField]
    private float readFast = 0.1f;
    [SerializeField]
    private KeyCode dialogueKeyCode = KeyCode.F;

    // Inspector Check
    public string names;
    private string[] dialogue;
    private bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool isaccept = false;
    public bool dialogueEnded = false;
    private bool inRange = false;
    [SerializeField]
    private bool isQuest = false;
    private float dialogueTime = 0;
    private int dialogueLine;
    private int currentLine;
    private int stringLength;
    private int currentString;

    // System
    public event Action OnStartDialogue;
    public event Action OnEndDialogue;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dialogueText.text = "";
    }

    public void EnterRangeOfNPC()
    {
        inRange = true;
        toChat.SetActive(true);
        if(dialogueActive == true)
        {
            toChat.SetActive(false);
        }
        isQuest = (quest == null) ? false : true;
    }

    public void StartDialogue(in string[] sentences)
    {
        inRange = true;
        toChat.SetActive(false);
        dialogueUI.SetActive(true);
        dialogue = sentences;
        nameText.text = names;
        if (Input.GetKeyDown(dialogueKeyCode) && !dialogueActive)
        {
            if (GameManager.Instance.IsGamePlaying)
            {
                GameManager.Instance.StopGame();
            }

            dialogueActive = true;
            OnStartDialogue?.Invoke();
            StartCoroutine(Dialogue());
        }
    }

    private IEnumerator Dialogue()
    {
        if (inRange == true)
        {
            dialogueLine = dialogue.Length;
            currentLine = 0;

            while (currentLine < dialogueLine || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogue[currentLine++]));

                    if (currentLine >= dialogueLine)
                    {
                        dialogueEnded = true;
                    }
                }
                yield return null;
            }

            while (true)
            {
                if (isQuest && quest.status == QuestStatus.None && dialogueEnded)
                {
                    QuestSelection();
                }
                else if (Input.GetKeyDown(dialogueKeyCode) && !dialogueEnded)
                {
                    break;
                }
                yield return null;
            }

            dialogueEnded = false;
            dialogueActive = false;
            OutOfRange();
        }
    }

    private IEnumerator DisplayString(string dialogueString)
    {
        if (inRange == true)
        {
            stringLength = dialogueString.Length;
            currentString = 0;

            dialogueText.text = "";

            while (currentString < stringLength)
            {
                dialogueText.text += dialogueString[currentString];
                currentString++;

                if (currentString < stringLength)
                {
                    if (Input.GetKey(dialogueKeyCode))
                    {
                        yield return WaitForUnscaledSeconds(readSpeed * readFast);
                    }
                    else
                    {
                        yield return WaitForUnscaledSeconds(readSpeed);
                    }
                }
                yield return null;
            }

            while (true)
            {
                if (isQuest && quest.status == QuestStatus.None && dialogueEnded)
                {
                    yield return null;
                }
                else if (Input.GetKeyDown(dialogueKeyCode))
                {
                    break;
                }
                yield return null;
            }

            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueText.text = "";
        }
    }

    private IEnumerator WaitForUnscaledSeconds(float time)
    {
        dialogueTime = 0f;
        while (dialogueTime < time)
        {
            yield return null;
            dialogueTime += Time.unscaledDeltaTime;
        }
    }

    public void DropDialogue()
    {
        dialogueEnded = false;
        dialogueActive = false;
        toChat.SetActive(true);
        dialogueUI.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        OnEndDialogue?.Invoke();

        if (!GameManager.Instance.IsGamePlaying)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    public void OutOfRange()
    {
        inRange = false;
        if (inRange == false)
        {
            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            toChat.SetActive(false);
            dialogueUI.SetActive(false);
            YesButton.SetActive(false);
            NoButton.SetActive(false);
        }

        if (!GameManager.Instance.IsGamePlaying)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    public void QuestSelection()
    {
        YesButton.SetActive(true);
        NoButton.SetActive(true);
    }

    public void OnClickYesButton()
    {
        isaccept = true;
        OutOfRange();
    }

    public void OnClickNoButton()
    {
        isaccept = false;
        OutOfRange();
    }
}
