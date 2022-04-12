using UnityEngine;
using TMPro;

public class MainSceneDebugUI : MonoBehaviour
{
    public TMP_InputField playerName;

    public void OnClickNewData()
    {
        Debug.Log(GameManager.Instance.NewGame(playerName.text));
    }

    public void OnClickSaveData()
    {
        GameManager.Instance.SaveGame();
    }

    public void OnClickLoadData()
    {
        GameManager.Instance.LoadGame(playerName.text);
    }
}
