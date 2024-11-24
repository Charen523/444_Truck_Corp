using System.Collections.Generic;

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

    private List<HeroData> heroList = new();
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

        // 타일맵에 등장
        TileMapManager.Instance.OnHeroEntered(data);
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

    public void AddTrainingSchedule(int heroIdx, int dummy)
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
            bool isSuccess = UnityEngine.Random.Range(0, 100) < s.successRate; //성공 여부
            if (isSuccess)
            {
                QuestData q = DataManager.Instance.GetData<QuestData>(nameof(QuestData), s.scheduleType);

                GameManager.Instance.OnGoldChangeEvent(q.rewardValues[0]);
                for (int i = 0; i < s.heroIdxs.Count; i++)
                {
                    heroList[s.heroIdxs[i]].GetExp(q.rewardValues[i]);
                }
            }

            for (int i = 0; i < s.heroIdxs.Count; i++)
            {
                TileMapManager.Instance.OnHeroEntered(heroList[s.heroIdxs[i]]);
                heroStates[s.heroIdxs[i]] = eHeroState.FREE;
                
            }
            scheduleList.Remove(s);
        }
        GameManager.Instance.OnFoodChangeEvent();
    }
}