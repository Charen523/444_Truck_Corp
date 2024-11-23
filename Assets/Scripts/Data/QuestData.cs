using System;

[Serializable]
public class QuestData
{
    public int id;
    public int difficulty;
    public string description;
    public string target;
    public string rewardType;
    public string rewardValue;
    public string needSpec;
    public int needTime;

    public string QuestName => description.Replace("N", target);
    public string[] rewardTypes => rewardType.Split('|');
    public int[] rewardValues => Array.ConvertAll(rewardValue.Split('|'), int.Parse);
    public int[]needSpecs => Array.ConvertAll(needSpec.Split('|'), int.Parse);
}