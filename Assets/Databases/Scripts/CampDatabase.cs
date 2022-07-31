using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampDatabase", menuName = "Database/Camp Database")]
public class CampDatabase : ScriptableObject
{
    public List<CampFriendliness> data;
}
