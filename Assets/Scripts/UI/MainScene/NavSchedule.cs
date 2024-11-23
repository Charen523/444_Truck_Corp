using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavSchedule : MonoBehaviour
{
    private int clickedSlotIdx = -1; //HeroSelect 이벤트를 적용할 칸.

    [SerializeField] PopupHeroSelect popupHeroSelect; //용사 선택창
    [SerializeField] PopupQuest popupQuest; //퀘스트 선택창
    
    [SerializeField] ScheduleHeroSlot[] scheduleHeroSlots = new ScheduleHeroSlot[3];
    [SerializeField] ScheduledQuestSlot scheduledQuestSlot;
    [SerializeField] Button questStartBtn;

    private bool[] isSelected = new bool[4];

    private void Awake()
    {
        GameManager.Instance.HeroSelectAction += SetCharacterSlot;
    }

    private void OnEnable()
    {
        clickedSlotIdx = -1;
        questStartBtn.interactable = false;

        for (int i = 0; i < 3; i++)
        {
            scheduleHeroSlots[i].InitSlot();
            isSelected[i] = false;
        }

        scheduledQuestSlot.InitSlot();
        isSelected[3] = false;
        StartCoroutine(EnableQuestStart());
    }

    #region Select Hero
    public void OnHeroSelectBtn(int idx)
    {
        clickedSlotIdx = idx;
        popupHeroSelect.gameObject.SetActive(true);
    }

    public void SetCharacterSlot(int heroIdx)
    {
        if (clickedSlotIdx == -1)
        {//방어코드
            Debug.LogWarning("슬롯 idx 이상함.");
            return;
        }

        HeroData heroData = HeroManager.Instance.GetHero(heroIdx);

        isSelected[clickedSlotIdx] = true;
        scheduleHeroSlots[clickedSlotIdx].SetScheduleSlot(heroData);

        scheduledQuestSlot.HeroSelected(clickedSlotIdx, heroData);
        scheduledQuestSlot.CalculateSuccessRate();
    }
    #endregion

    #region Select Quest
    public void OnQuestSelectBtn()
    {
        popupQuest.gameObject.SetActive(true);
    }

    public void SetQuestSlot(QuestData data)
    {
        isSelected[3] = true;
        scheduledQuestSlot.SetScheduleSlot(data);
    }
    #endregion

    #region Quest Start Btn
    private IEnumerator EnableQuestStart()
    {
        yield return new WaitUntil(() => isSelected[0] || isSelected[1] || isSelected[2]);
        yield return new WaitUntil(() => isSelected[3]);
        questStartBtn.interactable = true;
    }

    public void OnQuestStartBtn()
    {
        scheduledQuestSlot.ReturnScheduleInfo(out List<int> heroIdx, out QuestData qData, out int successRate);

        HeroManager.Instance.AddQuestSchedule(heroIdx, qData.id, GameManager.Instance.Day + qData.needTime, successRate);
        gameObject.SetActive(false);
    }
    #endregion

    public void OnBackBtn()
    {
        scheduledQuestSlot.ReturnHeroStates();
        gameObject.SetActive(false);
    }
}
