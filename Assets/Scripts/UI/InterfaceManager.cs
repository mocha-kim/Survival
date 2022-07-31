using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    private static InterfaceManager instance;
    public static InterfaceManager Instance => instance;

    public KeyCode EscapeKey => GameManager.Instance.escapeKey;

    public GameObject inventoryUI;
    public GameObject characterUI;
    public GameObject settingsUI;
    public GameObject questUI;
    public GameObject itemPocketUI;

    public List<GameObject> activeUIs = new();

    private SerializableDictionary<string, KeyCode> KeySettings => GameManager.Instance.gameData.keySettings;

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
        inventoryUI.SetActive(false);
        characterUI.SetActive(false);
        settingsUI.SetActive(false);
        questUI.SetActive(false);
        itemPocketUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(EscapeKey))
        {
            if (DialogueManager.Instance.dialogueActive)
            {
                DialogueManager.Instance.OutOfRange();
            }
            else if (activeUIs.Count > 0)
            {
                CloseUI(activeUIs[activeUIs.Count - 1]);
            }
            else
            {
                ToggleUI(settingsUI);
            }
        }

        if (Input.GetKeyDown(KeySettings[KeyName.Inventory.ToString()]) && GameManager.Instance.IsGamePlaying)
        {
            ToggleUI(inventoryUI);
        }
        if (Input.GetKeyDown(KeySettings[KeyName.Character.ToString()]) && GameManager.Instance.IsGamePlaying)
        {
            ToggleUI(characterUI);
        }
        if (Input.GetKeyDown(KeySettings[KeyName.Quest.ToString()]) && GameManager.Instance.IsGamePlaying)
        {
            ToggleUI(questUI);
        }
        if (Input.GetKeyDown(KeySettings[KeyName.Interact.ToString()]) && GameManager.Instance.IsGamePlaying)
        {

        }
    }

    public void OnFocus(GameObject go)
    {
        activeUIs.Remove(go);
        activeUIs.Add(go);

        go.transform.SetAsLastSibling();
    }

    public void ToggleUI(GameObject go)
    {
        if (go.activeSelf)
        {
            CloseUI(go);
        }
        else
        {
            OpenUI(go);
        }
    }

    private void CloseUI(GameObject go)
    {
        activeUIs.Remove(go);
        go.SetActive(false);

        if (go == settingsUI)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    private void OpenUI(GameObject go)
    {
        activeUIs.Add(go);
        go.SetActive(true);

        go.transform.SetAsLastSibling();

        if (go == settingsUI)
        {
            GameManager.Instance.StopGame();
        }
    }
}
