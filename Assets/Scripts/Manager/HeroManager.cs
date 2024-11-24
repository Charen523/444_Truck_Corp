using System.Collections.Generic;
using System.Linq;

public enum eHeroState
{
    FREE,
    QUEST,
    TRAINING
}

public class HeroManager : Singleton<HeroManager>
{
    private struct Schedule
    {
        public int scheduleType; // -1이면 Training, 0 이상 : QuestIdx.
        public List<int> heroIdxs;
        public int dDay;
        public int successRate;
    }

    public List<HeroData> heroList = new();
    public List<eHeroState> heroStates = new();
    private List<Schedule> scheduleList = new();

    private int heroCount = 0;

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.DayChangeAction += CheckScheduleDone;
    }

    public HeroData MakeNewHero()
    {
        HeroData data = new();
        data.Initialize(heroCount++);
        heroList.Add(data);
        heroStates.Add(eHeroState.FREE);
        GameManager.Instance.OnFoodChangeEvent();

        GameManager.Instance.OnHeroSpawnEvent?.Invoke(data);
        return data;
    }

    public HeroData GetHero(int index)
    {
        return heroList[index];
    }

    public void AddQuestSchedule(List<int> heroIdxs, int questIdx, int needTime, int successRate)
    {
        Schedule newS = new()
        {
            scheduleType = questIdx,
            heroIdxs = heroIdxs,
            dDay = GameManager.Instance.Day + needTime,
            successRate = successRate
        };
        scheduleList.Add(newS);

        foreach (int idx in heroIdxs)
        {
            heroStates[idx] = eHeroState.QUEST;

            // 타일맵 퇴장
            TileMapManager.Instance.OnHeroExit(heroList[idx]);
        }

        GameManager.Instance.OnFoodChangeEvent();
    }

    public void AddTrainingSchedule(int heroIdx)
    {
        heroStates[heroIdx] = eHeroState.TRAINING;
    }

    public void CheckTrainingScheduleDone(int heroIdx)
    {
        heroStates[heroIdx] = eHeroState.FREE;
    }

    public void CheckScheduleDone(int skipDay)
    {
        while (scheduleList.Count != 0 && scheduleList[0].dDay <= GameManager.Instance.Day)
        {
            Schedule s = scheduleList[0];
            bool isSuccess = UnityEngine.Random.Range(0, 100) < s.successRate; // 성공 여부
            QuestData q = DataManager.Instance.GetData<QuestData>(nameof(QuestData), s.scheduleType);

            var heroes = heroList.Where((hero) => s.heroIdxs.Contains(hero.id));
            GameManager.Instance.OnQuestEndEvent?.Invoke(heroes, q, isSuccess);
            // 퀘스트 성공
            if (isSuccess)
            {
                GameManager.Instance.OnGoldChangeEvent(q.rewardValues[0]);
                for (int i = 0; i < s.heroIdxs.Count; i++)
                {
                    heroList[s.heroIdxs[i]].GetExp(q.rewardValues[1]);
                }
            }
            // 퀘스트 실패
            else
            {
                // 용사 사망 처리
                for (int i = 0; i < s.heroIdxs.Count; i++)
                {
                    heroList[s.heroIdxs[i]].Dead();
                }
            }

            // 용사 상태 해제
            for (int i = 0; i < s.heroIdxs.Count; i++)
            {
                TileMapManager.Instance.OnHeroEntered(heroList[s.heroIdxs[i]]);
                heroStates[s.heroIdxs[i]] = eHeroState.FREE;
            }

            // 스케쥴 삭제
            scheduleList.Remove(s);
        }
        GameManager.Instance.OnFoodChangeEvent();
    }
}