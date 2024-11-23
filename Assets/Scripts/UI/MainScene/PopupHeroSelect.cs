using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupHeroSelect : MonoBehaviour
{
    [Header("Warn Popup")]
    public Transform warnParent;
    public GameObject warnPrefab;

    [Header("My Toggle Group")]
    public ToggleGroup toggleGroup;

    [Header("list obj")]
    [SerializeField] private Transform ListParent;
    [SerializeField] private GameObject ListPrefab;
    public Button heroSelectBtn;

    private List<HeroSelectSlot> HeroSlots = new(); //영웅 슬롯 캐싱.
    public int selectedHeroIdx; //초기값 : 0
    private bool isToggleOn;

    private void Awake()
    {
        //테스트 코드
        HeroManager.Instance.MakeNewHero();
        HeroManager.Instance.MakeNewHero();
        HeroManager.Instance.MakeNewHero();
    }

    private void OnEnable()
    {
        selectedHeroIdx = 0;
        isToggleOn = false;

        for (int i = 0; i < HeroManager.Instance.heroStates.Count; i++)
        {
            if (i >= HeroSlots.Count)
            {//신규 slot
                HeroSelectSlot newSlot = Instantiate(ListPrefab, ListParent).GetComponent<HeroSelectSlot>();
                HeroSlots.Add(newSlot);
                newSlot.InitHeroSlot(i, this, HeroManager.Instance.GetHero(i));
            }
            HeroSlots[i].SetHeroSlot(HeroManager.Instance.GetHero(i), ref isToggleOn);
        }

        if (!isToggleOn)
        {
            Instantiate(warnPrefab, warnParent)
                .GetComponent<PopupWarning>()
                .SetWarnTxt("선택 가능한 영웅이 없습니다.");
            gameObject.SetActive(false);
        }
    }

    public void OnSelectBtn()
    {
        GameManager.Instance.OnHeroSelectEvent(selectedHeroIdx);
        gameObject.SetActive(false);
    }
}
