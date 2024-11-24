using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NavSchedule : MonoBehaviour
{
    private int clickedSlotIdx = -1; //HeroSelect 이벤트를 적용할 칸.

    [SerializeField] PopupHeroSelect popupHeroSelect; //용사 선택창
    [SerializeField] PopupQuest popupQuest; //퀘스트 선택창

    [SerializeField] SlotScheduleHero[] scheduleHeroSlots = new SlotScheduleHero[3];
    [SerializeField] SlotScheduleQuest scheduledQuestSlot;
    [SerializeField] Button questStartBtn;

    [SerializeField] PotionToggleGroup toggleGroup;

    public int[] QuestSlotIdx { get; private set; } = new int[4];

    private bool[] isSelected = new bool[4];

    private void OnEnable()
    {
        GameManager.Instance.HeroSelectAction = SetHeroSlot;
        GameManager.Instance.QuestSelectAction = SetQuestSlot;

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

        int maxDiff = GameManager.Instance.Day switch
        {
            < -75 => 1,
            < -50 => 2,
            < -25 => 3,
            _ => 4
        };

        for (int i = 0; i < QuestSlotIdx.Length; i++)
        {
            QuestSlotIdx[i] = GetRandomQuest(maxDiff);
        }

        if (!GameManager.Instance.FirstQuest)
        {
            QuestSlotIdx[0] = 0;
        }
    }

    #region Select Hero
    public void OnHeroSelectBtn(int idx)
    {
        clickedSlotIdx = idx;
        popupHeroSelect.gameObject.SetActive(true);
    }

    public void SetHeroSlot(int heroIdx)
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
    private int GetRandomQuest(int maxDiff)
    {
        var filteredQuests = DataManager.Instance.GetDataList<QuestData>("QuestData")
            .Where(q => q.difficulty >= 1 && q.difficulty <= maxDiff)
            .ToList();

        if (filteredQuests.Count == 0)
        {
            Debug.LogWarning("조건에 맞는 퀘스트 난이도 오류");
            return -1;
        }

        int randomIndex = Random.Range(0, filteredQuests.Count);
        return randomIndex;
    }

    public void OnQuestSelectBtn()
    {
        popupQuest.gameObject.SetActive(true);
    }

    public void SetQuestSlot(int questIdx)
    {
        QuestData questData = DataManager.Instance.GetData<QuestData>(nameof(QuestData), questIdx);

        isSelected[3] = true;
        scheduledQuestSlot.SetScheduleSlot(questData);
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
        GameManager.Instance.UsePotion(toggleGroup.GetSelectedIndices());
        GameManager.Instance.OnPotionActionEvent();

        scheduledQuestSlot.ReturnScheduleInfo(out List<int> heroIdx, out QuestData qData, out int successRate);

        HeroManager.Instance.AddQuestSchedule(heroIdx, qData.id, qData.needTime, successRate);

        if (qData.id == 0) { GameManager.Instance.FirstQuest = true; }
        gameObject.SetActive(false);
    }
    #endregion

    public void OnBackBtn()
    {
        scheduledQuestSlot.ReturnHeroStates();
        gameObject.SetActive(false);
    }
}