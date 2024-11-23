using System;

[Serializable]
public class QuestData
{
    public int difficulty;
    public string description;
    public string target;
    public string rewardType;
    public string rewardValue;

    public string[] rewardTypes => rewardType.Split('|');
    public int[] rewardValues => Array.ConvertAll(rewardValue.Split('|'), int.Parse);
}