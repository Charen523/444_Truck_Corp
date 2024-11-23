
using System;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public Action<int> HeroSelectAction;

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
}