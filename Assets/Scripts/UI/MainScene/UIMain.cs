using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [Header("Warning")]
    [SerializeField] Transform warningParent;
    
    [Header("UIs")]
    [SerializeField] GameObject popupHeroList;
    [SerializeField] GameObject uiSchedule;
    [SerializeField] GameObject uiTraining;
    
    [Header("volume Setting")]
    [SerializeField] private Slider volumeSlider;

    [Header("Status")]
    [SerializeField] private TextMeshProUGUI curGoldTxt;
    [SerializeField] private TextMeshProUGUI dailyCostTxt;

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

        GameManager.Instance.SetWarnParent(warningParent);

        // AudioManager에 Slider 등록
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterSlider(volumeSlider);
        }
    }

    private void Start()
    {
        OnGoldChange(GameManager.Instance.gold);
        OnFoodChange(0);
        OnDayChange(GameManager.Instance.day); ;
    }

    #region Btns
    public void OnsummonBtn()
    {
        if (GameManager.Instance.gold < 100)
        {
            GameManager.Instance.InvokeWarning("골드가 부족합니다...");
            return;
        }
        else
        {
            HeroManager.Instance.MakeNewHero();
            GameManager.Instance.OnGoldChangeEvent(100);
        }
    }

    public void OnHeroListBtn()
    {
        if (popupHeroList != null)
            popupHeroList.SetActive(true);
    }

    public void OnScheduleBtn()
    {
        if (uiSchedule != null)
            uiSchedule.SetActive(true);
    }

    public void OnTrainingBtn()
    {
        if (uiTraining != null)
            uiTraining.SetActive(true);
    }
    #endregion

    #region Status
    private void OnGoldChange(int curGold)
    {//골드 변화
        curGoldTxt.text = curGold.ToString() + " 골드";
    }

    private void OnFoodChange(int heroCount)
    {//히어로 스케줄 변화
        dailyCostTxt.text = heroCount.ToString() + " 골드";
    }
    #endregion

    #region Time Skip
    public void IncreaseDay()
    {
        decreaseBtn.interactable = true;
        skipTxt.text = $"{++skipCount}일";
        if (skipCount + GameManager.Instance.day == 0)
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
        GameManager.Instance.OnDayChangeEvent(skipCount);
        skipCount = 1;
        skipTxt.text = $"{skipCount}일";
    }
    #endregion

    #region Date
    private void OnDayChange(int today)
    {
        dDayTxt.text = today.ToString();
    }
    #endregion
}