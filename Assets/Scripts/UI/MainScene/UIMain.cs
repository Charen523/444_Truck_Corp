using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [Header("volume Setting")]
    [SerializeField] private Slider volumeSlider;

    [Header("Status")]
    [SerializeField] private TextMeshProUGUI curGoldTxt;
    [SerializeField] private TextMeshProUGUI dailyCostTxt;

    [Header("Time Skip")]
    [SerializeField] private TextMeshProUGUI skipTxt;
    private int skipCount;

    [Header("Date")]
    [SerializeField] private TextMesh dDayTxt;

    private void Awake()
    {
        GameManager.Instance.GoldChangeAction += OnGoldChange;
    }

    private void Start()
    {
        // AudioManager에 Slider 등록
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterSlider(volumeSlider);
        }
    }

    #region Status
    private void OnGoldChange(int curGold)
    {//골드 변화
        curGoldTxt.text = curGold.ToString() + "골드";
    }

    private void OnLeftOverChange()
    {//히어로 스케줄 변화

    }
    #endregion

    #region Time Skip
    #endregion

    #region Date
    #endregion
}