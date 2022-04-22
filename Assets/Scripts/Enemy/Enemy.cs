using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Database/Enemy")]
public class Enemy : ScriptableObject
{
    public int id;
    public new string name;

    public float maxHP;
    public float currentHP;
    public float damage;

    public int rewardGold;
    public List<RewardItem> rewardItem;

    [System.Serializable]
    public struct RewardItem
    {
        public ItemObject item;
        public int count;
        [Range(0, 100)]
        public int percentage;
    }

    public Enemy()
    {
        id = -1;
    }
}
