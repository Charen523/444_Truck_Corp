using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [Header("Warning")]
    [SerializeField] Transform warningParent;
    
    [Header("UIs")]
    [SerializeField] GameObject navHeroList;
    [SerializeField] GameObject navSchedule;
    [SerializeField] GameObject navTraining;
    
    [Header("volume Setting")]
    [SerializeField] private Slider volumeSlider;

    [Header("Status")]
    [SerializeField] private TextMeshProUGUI curGoldTxt;
    [SerializeField] private TextMeshProUGUI FoodTxt;
    [SerializeField] private TextMeshProUGUI[] potionTxts;
    private int foodCost = 0;

    [Header("Time Skip")]
    [SerializeField] private TextMeshProUGUI skipTxt;
    [SerializeField] private Button decreaseBtn;
    [SerializeField] private Button increaseBtn;
    private int skipCount = 1;

    [Header("Date")]
    [SerializeField] private TextMeshProUGUI dDayTxt;

    private void Awake()
    {
        GameManager.Instance.GoldChangeAction += OnGoldChange;
        GameManager.Instance.FoodChangeAction += OnFoodChange;
        GameManager.Instance.DayChangeAction += OnDayChange;
        GameManager.Instance.PotionAction += OnPotionChange;

        GameManager.Instance.SetWarnParent(warningParent);
        GameManager.Instance.SetDialogParent(warningParent);

        // AudioManager에 Slider 등록
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterSlider(volumeSlider);
        }
    }

    private void Start()
    {
        OnGoldChange(GameManager.Instance.Gold);
        OnFoodChange();
        OnDayChange(GameManager.Instance.Day); ;
    }

    #region Btns
    public void OnsummonBtn()
    {
        if (GameManager.Instance.Gold < 100)
        {
            GameManager.Instance.InvokeWarning("골드가 부족합니다...");
            return;
        }
        else
        {
            HeroManager.Instance.MakeNewHero();
            GameManager.Instance.OnGoldChangeEvent(-100);
        }
    }

    public void OnHeroListBtn()
    {
        if (navHeroList != null)
            navHeroList.SetActive(true);
    }

    public void OnScheduleBtn()
    {
        if (navSchedule != null)
            navSchedule.SetActive(true);
    }

    public void OnTrainingBtn()
    {
        if (navTraining != null)
            navTraining.SetActive(true);
    }
    #endregion

    #region Status
    private void OnGoldChange(int curGold)
    {//골드 변화
        curGoldTxt.text = curGold.ToString() + " 골드";
    }

    private void OnFoodChange()
    {//히어로 스케줄 변화
        int heroCount = 0;
        foreach (var h in HeroManager.Instance.heroStates)
        {
            if (h != eHeroState.QUEST)
                heroCount++;
        }

        foodCost = heroCount * 50;
        FoodTxt.text = foodCost.ToString() + " 골드";
    }

    private void OnPotionChange()
    {
        for (int i = 0; i < potionTxts.Length; i++)
        {
            potionTxts[i].text = GameManager.Instance.Potions[i] + "개";
        }
    }
    #endregion

    #region Time Skip
    public void IncreaseDay()
    {
        decreaseBtn.interactable = true;
        skipTxt.text = $"{++skipCount}일";
        if (skipCount + GameManager.Instance.Day == 0)
        {
            increaseBtn.interactable = false;
        }
    }

    public void DecreaseDay()
    {
        increaseBtn.interactable = true;
        skipTxt.text = $"{--skipCount}일";
        if (skipCount == 1)
        {
            decreaseBtn.interactable = false;
        }
    }

    public void ChangeDate()
    {
        GameManager.Instance.OnGoldChangeEvent(-foodCost * skipCount);

        int trainCount = 0;
        foreach (var h in HeroManager.Instance.heroStates)
        {
            if (h == eHeroState.TRAINING) trainCount++;
        }
        GameManager.Instance.OnGoldChangeEvent(trainCount * skipCount);

        if (GameManager.Instance.Gold < 0)
        {
            GameManager.Instance.Ending = eEnding.Bankrupt;
            SceneManager.LoadScene(2);
        }

        GameManager.Instance.OnDayChangeEvent(skipCount);
        skipCount = 1;
        skipTxt.text = $"{skipCount}일";
        decreaseBtn.interactable = false;
        increaseBtn.interactable = true;
    }
    #endregion

    #region Date
    private void OnDayChange(int today)
    {
        dDayTxt.text = $"{-today}일";
    }
    #endregion
}