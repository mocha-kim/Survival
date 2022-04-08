using System;
using UnityEngine;

[Serializable]
public class Enemy
{
    public int id;
    public string name;

    public float maxHP;
    public float currentHP;
    public float damage;

    public float conExp;
    public float strExp;
    public float defExp;
}
