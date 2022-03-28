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
    public Text nameText;     // NPC Name
    public Text dialogueText; // NPC dialogue

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
    public bool isEnd = false;
    private bool dialogueEnded = false;
    private bool inRange = false;
    private float dialogueTime;
    private int lineLength;
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
        dialogueTime = 0;
    }

    public void EnterRangeOfNPC()
    {
        inRange = true;
        toChat.SetActive(true);
    }

    public void StartDialogue(in string[] sentences)
    {
        isEnd = false;
        dialogue = sentences;
        inRange = true;
        toChat.SetActive(false);
        dialogueUI.SetActive(true);
        nameText.text = names;
        if (Input.GetKeyDown(dialogueKeyCode) && !dialogueActive)
        {
            dialogueActive = true;
            OnStartDialogue?.Invoke();
            StartCoroutine(Dialogue());
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

    private IEnumerator Dialogue()
    {
        if (inRange == true)
        {
            lineLength = dialogue.Length;
            currentLine = 0;

            while (currentLine < lineLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogue[currentLine++]));

                    if (currentLine >= lineLength)
                    {
                        dialogueEnded = true;
                    }
                }
                yield return null;
            }

            while (true)
            {
                if (Input.GetKeyDown(dialogueKeyCode) && !dialogueEnded)
                {
                    break;
                }
                yield return null;
            }

            dialogueEnded = false;
            dialogueActive = false;
            DropDialogue();
        }
    }

    private IEnumerator DisplayString(string dialogueString)
    {
        if (GameManager.Instance.IsGamePlaying)
        {
            GameManager.Instance.StopGame();
        }

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
                else
                {
                    dialogueEnded = false;
                    break;
                }
                yield return null;
            }

            while (true)
            {
                if (Input.GetKeyDown(dialogueKeyCode))
                {
                    break;
                }
                yield return null;
            }

            isEnd = true;
            dialogueEnded = false;
            letterIsMultiplied = false;
        }
    }

    public void DropDialogue()
    {
        toChat.SetActive(false);
        dialogueUI.SetActive(false);
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
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            toChat.SetActive(false);
            dialogueUI.SetActive(false);
        }

        if (!GameManager.Instance.IsGamePlaying)
        {
            GameManager.Instance.ResumeGame();
        }
    }
}
