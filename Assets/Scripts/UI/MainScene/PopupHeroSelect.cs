using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupHeroSelect : MonoBehaviour
{
    [Header("My Toggle Group")]
    public ToggleGroup toggleGroup;

    [Header("list obj")]
    [SerializeField] private Transform ListParent;
    [SerializeField] private GameObject ListPrefab;
    public Button heroSelectBtn;

    private List<SlotHeroListBtn> HeroSlots = new(); //영웅 슬롯 캐싱.
    public int selectedHeroIdx; //초기값 : 0
    private bool isToggleOn;

    private void OnEnable()
    {
        selectedHeroIdx = 0;
        isToggleOn = false;

        for (int i = 0; i < HeroManager.Instance.heroStates.Count; i++)
        {
            if (i >= HeroSlots.Count)
            {//신규 slot
                SlotHeroListBtn newSlot = Instantiate(ListPrefab, ListParent).GetComponent<SlotHeroListBtn>();
                HeroSlots.Add(newSlot);
                newSlot.InitHeroSlot(i, this, HeroManager.Instance.GetHero(i));
            }
            HeroSlots[i].SetHeroSlot(HeroManager.Instance.GetHero(i), ref isToggleOn);
        }

        if (toggleGroup == null)
        {
            if (HeroManager.Instance.heroStates.Count == 0)
            {
                GameManager.Instance.InvokeWarning("아직 용사가 없습니다...");
                gameObject.SetActive(false);
            }
        }
        else if (!isToggleOn)
        {
            GameManager.Instance.InvokeWarning("남은 영웅이 없습니다...");
            gameObject.SetActive(false);
        }
    }

    public void OnSelectBtn()
    {
        GameManager.Instance.OnHeroSelectEvent(selectedHeroIdx);
        gameObject.SetActive(false);
    }
}
