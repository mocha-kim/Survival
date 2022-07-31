using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class KeySettingUI : UserInterface
{
    private readonly List<KeyCode> keyList = new List<KeyCode>
    {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P,
        KeyCode.LeftBracket, KeyCode.RightBracket, KeyCode.Backslash, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Semicolon, KeyCode.Quote, KeyCode.Z, KeyCode.X,
        KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M, KeyCode.Comma, KeyCode.Period
    };
    private readonly List<KeyCode> longKeyList = new List<KeyCode>
    {
        KeyCode.Tab, KeyCode.LeftShift, KeyCode.RightShift, KeyCode.LeftControl, KeyCode.RightControl,
        KeyCode.LeftAlt, KeyCode.RightAlt, KeyCode.Space
    };

    private Vector2 shortKeyScale = new(50f, 50f);
    private Vector2 longKeyScale = new(75f, 50f);

    private IEnumerator selectCo; 

    private bool isSelecting = false;
    private GameObject selectedGo;
    private KeyCode selectedKey;

    [SerializeField]
    private EventSystem eventSystem;

    private void OnEnable()
    {
        string[] keynames = Enum.GetNames(typeof(KeyName));
        TextMeshProUGUI[] tmps = transform.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 1; i < 14; i++)
        {
            tmps[i].text = GameManager.Instance.gameData.keySettings[keynames[i - 1]].ToString();
        }
    }

    protected override void Awake()
    {
        // do nothing
    }

    public void OnClickKey(string keyName)
    {
        selectedGo = eventSystem.currentSelectedGameObject;
        selectCo = SelectKey(keyName);
        isSelecting = true;
        StartCoroutine(selectCo);
    }

    public void OnDeselectKey()
    {
        StopCoroutine(selectCo);
    }

    IEnumerator SelectKey(string keyName)
    {
        while (isSelecting)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isSelecting = false;
                    yield break;
                }
                foreach (KeyCode key in keyList)
                {
                    if (Input.GetKeyDown(key))
                    {
                        selectedKey = key;
                        isSelecting = false;
                        if (ChangeKey(selectedGo, keyName, false))
                        {
                            yield break;
                        }
                    }
                }
                foreach (KeyCode key in longKeyList)
                {
                    if (Input.GetKeyDown(key))
                    {
                        selectedKey = key;
                        isSelecting = false;
                        if (ChangeKey(selectedGo, keyName, true))
                        {
                            yield break;
                        }
                        yield break;
                    }
                }
            }
            yield return null;
        }
        yield break;
    }

    private bool ChangeKey(GameObject go, string keyName, bool isLongkey)
    {
        foreach (KeyValuePair<string, KeyCode> pair in GameManager.Instance.gameData.keySettings)
        {
            if (pair.Value == selectedKey)
            {
                eventSystem.SetSelectedGameObject(null);
                return false;
            }
        }

        if (isLongkey)
        {
            go.GetComponent<RectTransform>().sizeDelta = longKeyScale;
        }
        else
        {
            go.GetComponent<RectTransform>().sizeDelta = shortKeyScale;
        }
        go.GetComponentInChildren<TextMeshProUGUI>().text = selectedKey.ToString();
        eventSystem.SetSelectedGameObject(null);
        Debug.Log("Change " + keyName + " key: " + GameManager.Instance.gameData.keySettings[keyName].ToString() + " to " + selectedKey);
        GameManager.Instance.gameData.keySettings[keyName] = selectedKey;
        return true;
    }
}
