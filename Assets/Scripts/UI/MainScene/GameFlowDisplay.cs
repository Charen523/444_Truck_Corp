using System.Collections.Generic;
using UnityEngine;

public enum GameFlowEventType
{
    HeroSpawn,
    LevelUp,
    GetExp,
    StatUp,
    GetGold,
    HeroDead,
}

public class GameFlowDisplay : MonoBehaviour
{
    [SerializeField] private Transform toastParent;

    private int remainDays;
    private Queue<GameFlowEvent> queue = new Queue<GameFlowEvent>();

    private void Start()
    {
        GameManager.Instance.OnHeroLevelUpEvent += OnHeroLevelUp;
        GameManager.Instance.OnGetExpEvent += OnHeroGetExp;
        GameManager.Instance.OnHeroStatUpEvent += OnHeroStatUp;
        GameManager.Instance.GoldChangeAction += OnGoldChange;
        GameManager.Instance.OnHeroDeadEvent += OnHeroDead;
        GameManager.Instance.OnHeroSpawnEvent += OnHeroSpawn;
    }

    private void OnHeroSpawn(HeroData hero)
    {
        string message = $"{hero.classData.className} \"{hero.name}\"이(가) 소환했습니다.";
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.HeroSpawn, message));
    }

    private void OnHeroLevelUp(HeroData hero, int level)
    {
        string message = $"\"{hero.name}\"이(가) \'{level}\'레벨을 달성했습니다.";
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.LevelUp, message));
    }

    private void OnHeroGetExp(HeroData hero, int exp)
    {
        string message = $"\"{hero.name}\"이(가) 경험치 \'{exp}\'을(를) 획득했습니다.";
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.GetExp, message));
    }

    private void OnHeroStatUp(HeroData hero, Status status)
    {
        string message = $"\"{hero.name}\"의 ";

        List<string> statIncreases = new List<string>();

        if (status.STR > 0)
        {
            statIncreases.Add($"STR 스탯이 {status.STR}");
        }
        if (status.DEX > 0)
        {
            statIncreases.Add($"DEX 스탯이 {status.DEX}");
        }
        if (status.INT > 0)
        {
            statIncreases.Add($"INT 스탯이 {status.INT}");
        }
        if (status.LUK > 0)
        {
            statIncreases.Add($"LUK 스탯이 {status.LUK}");
        }

        if (statIncreases.Count > 0)
        {
            message += string.Join(", ", statIncreases);
            message += "증가했습니다.";
            queue.Enqueue(new GameFlowEvent(GameFlowEventType.StatUp, message));
        }
        //else
        //{
        //    message = " 스탯 변화가 없습니다.";
        //}
    }

    private void OnGoldChange(int gold)
    {
        string message = $"{gold} 골드를 " + ((gold > 0) ? "획득했습니다." : "지불했습니다.");
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.GetGold, message));
    }

    private void OnHeroDead(HeroData hero)
    {
        string message = $"{hero.name} 이(가) 사망했습니다.";
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.HeroDead, message));
    }

    public void OnDayChanged(int delta)
    {
        // 화면 페이드 아웃
        // 
        remainDays = delta;

        //
    }

    private float changingDayDelayTime = 1.0f;
    private float displayDelayTime = 0.25f;
    private float time;

    private void FixedUpdate()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            return;
        }

        if (queue.Count > 0)
        {
            GameFlowEvent gameFlowEvent = queue.Dequeue();
            // UI에 추가
            var toast = PoolManager.Instance.Get<ToastMessage>("Prefabs/ToastMessage", toastParent);
            toast.Initialize(gameFlowEvent.Message);
            Canvas.ForceUpdateCanvases();
            time = displayDelayTime;
            return;
        }

        if (remainDays > 0)
        {
            remainDays--;
            time = changingDayDelayTime;
        }

        // 이벤트 종류

        // 퀘스트
        // - 성공
        // - 실패

        // 훈련장
        // - 스탯업
    }
}

public class GameFlowEvent
{
    public GameFlowEventType GameFlowEventType;
    public string Message;

    public GameFlowEvent(GameFlowEventType type, string message)
    {
        GameFlowEventType = type;
        Message = message;
    }
}