using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "Database/NPC")]
public class NPC : ScriptableObject
{
    public int id;
    public new string name;
    public CampType camp;

    public int friendliness;

    public NPC()
    {
        id = -1;
    }
}
