using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public GameDataContainer gameData;

    public StatsObject playerStats;
    public InventoryObject inventory;
    public InventoryObject quickslot;

    public KeyCode escapeKey = KeyCode.Escape;

    [SerializeField]
    private ItemDatabase itemDatabase;
    [SerializeField]
    private EnemyDatabase enemyDatabase;

    public Action OnGameManagerStart;

    private bool isGamePlaying = true;

    public bool IsGamePlaying
    {
        get
        {
            return isGamePlaying;
        }
    }

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
        LoadGame(gameData.playerName);
        OnGameManagerStart?.Invoke();
    }

    private void Update()
    {
        foreach (Condition condition in playerStats.conditions.Values)
        {
            if (condition.isActive)
            {
                if (condition.needTreatment) continue;
                condition.activationTime -= Time.deltaTime;
                if (condition.activationTime <= 0)
                {
                    playerStats.DeactivateCondition(condition);
                }
            }
        }
    }

    public string GetItemName(int id) => itemDatabase.itemObjects.FirstOrDefault(i => i.data.id == id)?.data.name;
    public bool IsItemStackable(int itemId) => itemDatabase.itemObjects.FirstOrDefault(i => i.data.id == itemId).isStackable;

    public string GetEnemyName(int id) => enemyDatabase.data.FirstOrDefault(i => i.id == id)?.name;
    public Enemy GetEnemyData(int id) => enemyDatabase.data.FirstOrDefault(i => i.id == id);
    public Enemy GetEnemyData(string name) => enemyDatabase.data.FirstOrDefault(i => i.name == name);


    public int GetTotalItemCount(int id)
    {
        int count = 0;
        if (inventory.IsContain(id))
        {
            count += inventory.CountItem(id);
        }
        if (quickslot.IsContain(id))
        {
            count += quickslot.CountItem(id);
        }
        return count;
    }

    public void StopPlayer()
    {
        isGamePlaying = false;
    }

    public void ResumePlayer()
    {
        isGamePlaying = true;
    }

    public void StopGame()
    {
        Time.timeScale = 0f;
        isGamePlaying = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePlaying = true;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public SaveLoad.Code NewGame(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName) || playerName.Length > 10) return SaveLoad.Code.Empty;
        if (SaveLoad.IsAlreadyExist(Application.persistentDataPath + "/PlayerData/" + playerName + "_s")) return SaveLoad.Code.AlreadyExist;

        bool success = true;

        gameData.ResetData(playerName);
        playerStats.ResetStats();
        QuestManager.Instance.playerQuests.ResetQuests();
        inventory.ClearInventory();
        quickslot.ClearInventory();

        SaveLoad.CreateDataDirectory();
        success = SaveLoad.SaveData(gameData, "/PlayerData/" + playerName + "_g") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(playerStats, "/PlayerData/" + playerName + "_s") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(QuestManager.Instance.playerQuests, "/PlayerData/" + playerName + "_q") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(inventory, "/PlayerData/" + playerName + "_i") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(quickslot, "/PlayerData/" + playerName + "_qs") == SaveLoad.Code.Success && success;

        if (success)
        {
            gameData.playerName = playerName;
            Debug.Log("! Game Data Created !");
            return SaveLoad.Code.Success;
        }
        else
        {
            Debug.Log("Game Data Creation Failed");
            return SaveLoad.Code.Failed;
        }
    }

    public void ResetGame()
    {
        gameData.ResetData(gameData.playerName);
        playerStats.ResetStats();
        QuestManager.Instance.playerQuests.ResetQuests();
        inventory.ClearInventory();
        quickslot.ClearInventory();

        SaveGame();
    }

    public void SaveGame()
    {
        bool success = true;

        success = SaveLoad.SaveData(gameData, "/PlayerData/" + gameData.playerName + "_g") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(playerStats, "/PlayerData/" + gameData.playerName + "_s") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(QuestManager.Instance.playerQuests, "/PlayerData/" + gameData.playerName + "_q") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(inventory, "/PlayerData/" + gameData.playerName + "_i") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(quickslot, "/PlayerData/" + gameData.playerName + "_qs") == SaveLoad.Code.Success && success;

        if (success)
        {
            Debug.Log("! Game Data Saved !");
        }
        else
        {
            Debug.Log("Game Data Save failed");
        }
    }

    public void LoadGame(string playerName)
    {
        bool success = true;

        success = SaveLoad.LoadData(gameData, "/PlayerData/" + playerName + "_g") == SaveLoad.Code.Success && success;
        success = SaveLoad.LoadData(playerStats, "/PlayerData/" + playerName + "_s") == SaveLoad.Code.Success && success;
        success = SaveLoad.LoadData(QuestManager.Instance.playerQuests, "/PlayerData/" + playerName + "_q") == SaveLoad.Code.Success && success;
        success = SaveLoad.LoadData(inventory, "/PlayerData/" + playerName + "_i") == SaveLoad.Code.Success && success;
        success = SaveLoad.LoadData(quickslot, "/PlayerData/" + playerName + "_qs") == SaveLoad.Code.Success && success;

        if (success)
        {
            gameData.playerName = playerName;
            Debug.Log("! Game Data Loaded !");
        }
        else
        {
            Debug.Log("Game Data Loading failed");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
