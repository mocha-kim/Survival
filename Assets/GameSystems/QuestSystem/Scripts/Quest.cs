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
    public int[] rewardItemIds;
    public int[] rewardItemCounts;

    public Quest()
    {
        id = -1;
    }
}
