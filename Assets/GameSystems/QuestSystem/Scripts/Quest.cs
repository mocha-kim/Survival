using System;

[Serializable]
public class Quest
{
    public int id;
    public int targetID;

    public int currentCount;
    public int goalCount;

    public int rewardGold;
    public int[] rewardItemIds;

    public Quest()
    {
        id = -1;
    }
}
