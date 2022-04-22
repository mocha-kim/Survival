using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSettingUI : UserInterface
{
    [SerializeField]
    private GameObject resetAlertWindow;
    [SerializeField]
    private GameObject exitAlertWindow;

    public void OnClickResetGame()
    {
        resetAlertWindow.SetActive(true);
    }

    public void OnClickResetYes()
    {
        GameManager.Instance.ResetGame();
    }

    public void OnClickResetNo()
    {
        resetAlertWindow.SetActive(false);
    }

    public void OnClickSaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    public void OnClickExitGame()
    {
        exitAlertWindow.SetActive(true);
    }

    public void OnClickExitYes()
    {
        GameManager.Instance.ExitGame();
    }

    public void OnClickExitNo()
    {
        exitAlertWindow.SetActive(false);
    }
}
