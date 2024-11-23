
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
    [SerializeField] private GameObject popupWarning;

    public int Day { get; private set; }
    public int Gold { get; private set; }

    private void Start()
    {
        Day = -100;
        Gold = 100;
    }

    #region event invoker
    public void OnHeroSelectEvent(int idx)
    {
        HeroSelectAction?.Invoke(idx);
    }

    public void OnGoldChangeEvent(int delta)
    {
        Gold += delta;
        GoldChangeAction?.Invoke(Gold);
    }

    public void OnFoodChangeEvent(int heroCount)
    {
        FoodChangeAction?.Invoke(heroCount);
    }

    public void OnDayChangeEvent(int delta)
    {
        Day += delta;
        DayChangeAction?.Invoke(Day);
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