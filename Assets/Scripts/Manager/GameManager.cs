
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Action<int> HeroSelectAction;
    public Action<int> GoldChangeAction;
    public Action<int> FoodChangeAction;
    public Action<int> DayChangeAction;

    private Transform warnParent;
    private GameObject popupWarning;

    public int day;
    public int gold;

    private void Start()
    {
        day = -100;
        gold = 100;
    }

    #region event invoker
    public void OnHeroSelectEvent(int idx)
    {
        HeroSelectAction?.Invoke(idx);
    }

    public void OnGoldChangeEvent(int delta)
    {
        gold += delta;
        GoldChangeAction?.Invoke(gold);
    }

    public void OnFoodChangeEvent(int heroCount)
    {
        FoodChangeAction?.Invoke(heroCount);
    }

    public void OnDayChangeEvent(int delta)
    {
        day += delta;
        DayChangeAction?.Invoke(day);
    }
    #endregion

    #region warning
    public void SetWarnParent(Transform t)
    {
        warnParent = t;
    }

    public void InvokeWarning(string msg)
    {
        Instantiate(popupWarning, warnParent)
                .GetComponent<PopupWarning>()
                .SetWarnTxt(msg);
    }
    #endregion
}