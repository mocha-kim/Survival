using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public string playerName = "";

    public StatsObject playerStats;
    public InventoryObject inventory;
    public InventoryObject quickslot;

    public int selectedQuickslot = 0;

    public ItemDatabase itemDatabase;
    public EnemyDatabase enemyDatabase;

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

    public string GetEnemyName(int id) => enemyDatabase.datas.FirstOrDefault(i => i.id == id)?.name;
    public Enemy GetEnemyData(int id) => enemyDatabase.datas.FirstOrDefault(i => i.id == id);
    public Enemy GetEnemyData(string name) => enemyDatabase.datas.FirstOrDefault(i => i.name == name);


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

        playerStats.ResetStats();
        QuestManager.Instance.playerQuests.ResetQuests();
        inventory.ClearInventory();
        quickslot.ClearInventory();

        SaveLoad.CreateDataDerectory();
        success = SaveLoad.SaveData(playerStats, "/PlayerData/" + playerName + "_s") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(QuestManager.Instance.playerQuests, "/PlayerData/" + playerName + "_q") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(inventory, "/PlayerData/" + playerName + "_i") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(quickslot, "/PlayerData/" + playerName + "_qs") == SaveLoad.Code.Success && success;

        if (success)
        {
            this.playerName = playerName;
            Debug.Log("! Game Data Created !");
            return SaveLoad.Code.Success;
        }
        else
        {
            Debug.Log("Game Data Creation Failed");
            return SaveLoad.Code.Failed;
        }
    }

    public void SaveGame()
    {
        bool success = true;

        success = SaveLoad.SaveData(playerStats, "/PlayerData/" + playerName + "_s") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(QuestManager.Instance.playerQuests, "/PlayerData/" + playerName + "_q") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(inventory, "/PlayerData/" + playerName + "_i") == SaveLoad.Code.Success && success;
        success = SaveLoad.SaveData(quickslot, "/PlayerData/" + playerName + "_qs") == SaveLoad.Code.Success && success;

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

        success = SaveLoad.LoadData(playerStats, "/PlayerData/" + playerName + "_s") == SaveLoad.Code.Success && success;
        success = SaveLoad.LoadData(QuestManager.Instance.playerQuests, "/PlayerData/" + playerName + "_q") == SaveLoad.Code.Success && success;
        success = SaveLoad.LoadData(inventory, "/PlayerData/" + playerName + "_i") == SaveLoad.Code.Success && success;
        success = SaveLoad.LoadData(quickslot, "/PlayerData/" + playerName + "_qs") == SaveLoad.Code.Success && success;

        if (success)
        {
            this.playerName = playerName;
            Debug.Log("! Game Data Loaded !");
        }
        else
        {
            Debug.Log("Game Data Loading failed");
        }
    }
}
