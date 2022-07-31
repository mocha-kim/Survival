using System;
using System.Collections.Generic;

[Serializable]
public class Quest
{
    public int id;
    public int targetID;
    public int destNPCID;

    public int currentCount;
    public int goalCount;

    public int rewardGold;
    public List<int> rewardItemIds = new();
    public List<int> rewardItemCounts = new();
    public List<Friendliness> updateFriendliness;

    [Serializable]
    public struct Friendliness
    {
        public CampFriendliness camp;
        public int addFriendliness;
    }

    public Quest()
    {
        id = -1;
    }
}
