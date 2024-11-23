using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class SlotScheduleQuest : MonoBehaviour
{
    [SerializeField] private GameObject nullBtn;
    [SerializeField] private GameObject info;

    [SerializeField] private TextMeshProUGUI questNameTxt;
    [SerializeField] private TextMeshProUGUI neededStatTxt;
    [SerializeField] private TextMeshProUGUI successRateTxt;

    /*스케줄 정보*/
    private HeroData[] selectedHero = new HeroData[3]; 
    private QuestData selectedQuest;
    private int successRate;

    public void InitSlot()
    {//초기화 (NavSChedule OnEnable)
        nullBtn.SetActive(false); //빈 퀘스트 선택 버튼.
        info.SetActive(false); //내용UI + 퀘스트 선택 버튼.

        for (int i = 0; i < selectedHero.Length; i++)
        {//데이터 초기화.
            selectedHero[i] = null;
            selectedQuest = null;
            successRate = 0;
        }
    }

    public void HeroSelected(int idx, HeroData hero)
    { //스케줄표에 선택된 히어로 정보 저장.
        nullBtn.SetActive(true);
        
        if (selectedHero[idx] != null)
        {//현재 스케줄에서 선택된 캐릭터 풀어주기
            HeroManager.Instance.heroStates[selectedHero[idx].id] = eHeroState.FREE;
        }
        
        selectedHero[idx] = hero;
        HeroManager.Instance.heroStates[selectedHero[idx].id] = eHeroState.QUEST;
    }

    public void SetScheduleSlot(QuestData quest)
    {
        nullBtn.SetActive(false);
        info.SetActive(true);

        selectedQuest = quest;

        questNameTxt.text = selectedQuest.QuestName;
        neededStatTxt.text = $"STR - {selectedQuest.needSpecs[0]}, DEF - {selectedQuest.needSpecs[1]}, INT - {selectedQuest.needSpecs[2]}";

        CalculateSuccessRate();
    }

    public void CalculateSuccessRate()
    {
        if (selectedQuest != null)
        {
            float rate = 0;
            float _str = 0, _dex = 0, _int = 0;

            //TODO: 성공률 계산식.
            for (int i = 0; i < selectedHero.Length; i++)
            {
                if (selectedHero[i] != null)
                {
                    _str += selectedHero[i].status.STR;
                    _dex += selectedHero[i].status.DEX;
                    _int += selectedHero[i].status.INT;
                }
            }
            _str = Mathf.Min(_str / selectedQuest.needSpec[0], 1);
            _dex = Mathf.Min(_dex / selectedQuest.needSpec[1], 1);
            _int = Mathf.Min(_int / selectedQuest.needSpec[2], 1);

            rate = _str * _dex * _int;
            rate = Mathf.RoundToInt(rate * 100);
            successRate = (int)rate;
            successRateTxt.text = $"{successRate}%";
        }
    }

    public void ReturnScheduleInfo(out List<int> list, out QuestData qData, out int rate)
    {
        list = new();
        for (int i = 0; i < selectedHero.Length; i++)
        {
            list.Add(selectedHero[i].id);
        }
        qData = selectedQuest;
        rate = successRate;
    }

    public void ReturnHeroStates()
    {
        for (int i = 0;i < selectedHero.Length;i++)
        {
            if (selectedHero[i] != null)
            {
                HeroManager.Instance.heroStates[selectedHero[i].id] = eHeroState.FREE;
            }
        }
    }
}