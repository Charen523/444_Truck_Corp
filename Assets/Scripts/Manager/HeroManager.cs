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

    public HeroData MakeNewHero()
    {
        HeroData data = new();
        data.Initialize(heroCount++);
        heroList.Add(data);
        heroStates.Add(eHeroState.FREE);
        return data;
    }

    public HeroData GetHero(int index)
    {
        return heroList[index];
    }

    public void AddQuestSchedule(List<int> heroIdxs, int questIdx, int dDay, int successRate)
    {
        Schedule newS = new()
        {
            scheduleType = questIdx,
            heroIdxs = heroIdxs,
            dDay = dDay,
            successRate = successRate
        };
        scheduleList.Add(newS);

        foreach (int idx in heroIdxs) 
        {
            heroStates[idx] = eHeroState.QUEST;
        }

        int leftHeroCount = 0;
        for (int i = 0; i < heroStates.Count; i++)
        {
            if (heroStates[i] != eHeroState.QUEST)
            {
                leftHeroCount++;
            }
        }
        GameManager.Instance.OnFoodChangeEvent(leftHeroCount);
    }

    public void AddTrainingSchedule(int heroIdx, int dDay, int successRate)
    {
        Schedule newS = new()
        {
            scheduleType = -1,
            heroIdxs = new() { heroIdx },
            dDay = dDay,
            successRate = successRate
        };
        scheduleList.Add(newS);

        heroStates[heroIdx] = eHeroState.TRAINING;
    }

    //TODO: 날짜 스킵과 연결.
    public void CheckScheduleDone(int today)
    {
        foreach (Schedule s in scheduleList)
        {
            if (s.dDay > today) break; //날짜 안지남

            /*보상 로직.*/
            if (s.scheduleType == -1)
            {
                //훈련 보상
            }
            else
            {//퀘스트 보상
                bool isSuccess = UnityEngine.Random.Range(0, 100) < s.successRate; //성공 여부
                if (isSuccess)
                {
                    QuestData q = (QuestData)DataManager.Instance.GetData("QuestData", s.scheduleType);

                    //골드 보상
                    GameManager.Instance.gold += q.rewardValues[0];
                    //경험치 보상
                    if (q.rewardValues.Length == 2)
                    {
                        for (int i = 0; i < s.heroIdxs.Count; i++)
                        {
                            heroList[s.heroIdxs[i]].exp += q.rewardValues[i];
                        }
                    }
                }
            }

            for (int i = 0; i < s.heroIdxs.Count; i++)
            {
                heroStates[s.heroIdxs[i]] = eHeroState.FREE;
            }
            scheduleList.Remove(s);
        }
    }
}