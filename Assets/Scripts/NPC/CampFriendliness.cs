using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampFriendliness", menuName = "Database/Camp Friendliness")]
public class CampFriendliness : ScriptableObject
{
    public CampType camp;
    public int friendliness;
}
