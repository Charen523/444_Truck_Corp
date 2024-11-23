
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Action<int> HeroSelectAction;
    public Action<int> GoldChangeAction;
    public Action<int> FoodChangeAction;

    public int day;
    public int gold;

    private void Start()
    {
        day = 1;
        gold = 0;
    }

    

    public void OnHeroSelectEvent(int idx)
    {
        HeroSelectAction?.Invoke(idx);
    }

    public void OnGoldChangeEvent(int delta)
    {
        gold += delta;
        GoldChangeAction?.Invoke(gold);
    }
}