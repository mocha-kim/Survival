using System;
using UnityEngine;

[Serializable]
public class NPC
{
    public int id;
    public string name;
    public CampType camp;

    public int maxHP;
    public int damage;

    public int friendliness;
    public string quest;
    public string dialogue;
}
