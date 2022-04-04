using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public StatsObject playerStats;
    public InventoryObject inventory;
    public InventoryObject quickslot;

    public bool isNewPlayer = true;

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
}
