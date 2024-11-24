using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eEnding
{
    Bankrupt,
    Lose,
    Win
}

public class GameManager : Singleton<GameManager>
{
    public Action<int> HeroSelectAction;
    public Action<int> QuestSelectAction;

    public Action<int> GoldChangeAction;
    public Action FoodChangeAction;
    public Action<int> DayChangeAction;
    public Action PotionAction;

    private Transform warnParent;
    private Transform dialogParent;
    [SerializeField] private GameObject popupWarning;
    [SerializeField] private GameObject popupDialog;

    public int Day { get; private set; }
    public int Gold { get; private set; }
    public int[] Potions { get; private set; } = new int[4];
    public eEnding Ending { get; set; }
    public bool FirstQuest { get; set; }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Day = -100;
        Gold = 500;

        for (int i = 0; i < Potions.Length; i++)
        {
            Potions[i] = 5;
        }

        FirstQuest = false;
    }

    #region event invoker
    public void OnHeroSelectEvent(int idx)
    {
        HeroSelectAction?.Invoke(idx);
    }

    public void OnQuestSelectEvent(int idx)
    {
        QuestSelectAction?.Invoke(idx);
    }

    public void OnGoldChangeEvent(int delta)
    {
        Gold += delta;
        GoldChangeAction?.Invoke(Gold);
    }

    public void OnFoodChangeEvent()
    {
        FoodChangeAction?.Invoke();
    }

    public void OnDayChangeEvent(int delta)
    {
        Day += delta;
        DayChangeAction?.Invoke(Day);

        if (Day == 0)
        {
            //TODO: 엔딩 분기 계산
            Ending = eEnding.Lose;
            SceneManager.LoadScene(2);
        }
    }

    public void OnPotionActionEvent()
    {
        PotionAction?.Invoke();
    }
    #endregion

    public void UsePotion(int[] potionIdxs)
    {
        for (int i = 0; i < potionIdxs.Length; i++)
        {
            Potions[potionIdxs[i]]--;
        }
    }

    #region warning and dialog
    public void SetWarnParent(Transform t)
    {
        warnParent = t;
    }

    public void SetDialogParent(Transform t)
    {
        dialogParent = t;
    }

    public void InvokeWarning(string msg, string title = null)
    {
        Instantiate(popupWarning, warnParent)
                .GetComponent<PopupWarning>()
                .SetWarnTxt(msg, title);
    }

    public void InvokeDialog(string msg, Action<eDialogResult> action)
    {
        Instantiate(popupDialog, dialogParent)
            .GetComponent<PopupDialog>()
            .SetTextAndAction(msg, action);
    }
    #endregion
}