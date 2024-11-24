
using System;

[Serializable]
public class ClassData
{
    public int id;
    public string className;

    // 기본 스탯
    public int baseStr;
    public int baseDex;
    public int baseInt;
    public int baseCon;
    public int baseLuk;

    // 증가 스탯
    public int incStr;
    public int incDex;
    public int incInt;
    public int incCon;
    public int incLuk;

    public Status BaseStat => new Status(baseStr, baseDex, baseInt, baseCon, baseLuk);
    public Status IncStat => new Status(incStr, incDex, incInt, incCon, incLuk);
}