using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum GameFlowEventType
{
    HeroSpawn,
    LevelUp,
    GetExp,
    StatUp,
    GetGold,
    HeroDead,
    HeroFeed,
    QuestEnd,
    QuestStart,
    HeroTrainingGold,
    HeroTrainingStatUp,
}

public class GameFlowDisplay : MonoBehaviour
{
    [SerializeField] private Transform toastParent;
    [SerializeField] private Image screenCover;
    [SerializeField] private UIMain uiMain;

    private int remainDays;
    private RectTransform toastParentRect;
    private Queue<GameFlowEvent> queue = new Queue<GameFlowEvent>();

    private void Start()
    {
        toastParentRect = (RectTransform)toastParent;

        // 이벤트 등록
        GameManager.Instance.OnHeroLevelUpEvent += OnHeroLevelUp;
        GameManager.Instance.OnGetExpEvent += OnHeroGetExp;
        GameManager.Instance.OnHeroStatUpEvent += OnHeroStatUp;
        GameManager.Instance.OnGetGoldEvent += OnGetGold;
        GameManager.Instance.OnHeroDeadEvent += OnHeroDead;
        GameManager.Instance.OnHeroSpawnEvent += OnHeroSpawn;
        GameManager.Instance.OnHeroFeedEvent += OnHeroFeed;
        GameManager.Instance.OnQuestStartEvent += OnQuestStart;
        GameManager.Instance.OnQuestEndEvent += OnQuestEnd;
        GameManager.Instance.OnHeroTrainingGoldEvent += OnHeroTraining;
        GameManager.Instance.OnHeroTrainingStatUpEvent += OnHeroTrainingStatUp;

        GameManager.Instance.OnDayChangeButtonEvent += OnDayChanged;
    }

    private void OnHeroTrainingStatUp(HeroData hero, string statText, int value)
    {
        string message = $"훈련의 성과로 {hero.name}의 {statText}이 {value} 상승했다!";

    }

    private void OnQuestStart(IEnumerable<HeroData> heroes, QuestData questData)
    {
        string message = string.Join(", ", heroes.Select((hero) => hero.name))
            + $"이(가) {questData.QuestName}을(를) 시작했습니다.";
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.QuestStart, message));
    }

    private void OnQuestEnd(IEnumerable<HeroData> heroes, QuestData questData, bool isSuccess)
    {
        string message = string.Join(", ", heroes.Select((hero) => hero.name))
            + $"이(가) {questData.QuestName}을(를) ";
        message += (isSuccess) ? "성공했습니다!" : "실패했습니다.";
        // 스크롤, 확률, 자동편성, 정렬, 초기버그, 용사목록(일정, 정렬), 식대 표시, 다시하기, 저장
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.QuestEnd, message));
    }

    private void OnHeroFeed(int value)
    {
        string message = $"식비로 {value} 골드를 지불했습니다.";
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.HeroFeed, message));
    }

    private void OnHeroTraining(int value)
    {
        string message = $"훈련비로 {value} 골드를 지불했습니다.";
        queue.Enqueue(new GameFlowEvent(GameFlowEventType.HeroFeed, message));
    }

    private void OnHeroSpawn(HeroData hero)
    {
        string message = $"{hero.classData.className} \"{hero.name}\"이(가) 소환되었습니다.";
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
    }

    private void OnGetGold(int gold)
    {
        string message = $"{Mathf.Abs(gold)} 골드를 " + ((gold > 0) ? "획득했습니다." : "지불했습니다.");
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
        screenCover.DOFade(1.0f, 1.0f).OnComplete(() => isFading = false);
        isFading = true;
        remainDays = delta;
    }

    private float changingDayDelayTime = 1.0f;
    private float displayDelayTime = 0.25f;
    private float time;
    private float position;
    private bool isFading;

    private void FixedUpdate()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            return;
        }

        // 출력할 메세지가 있다면 출력
        if (TryDisplayMessage())
        {
            time = displayDelayTime;
            return;
        }

        // 변경할 날짜가 남아있다면 변경
        if (TryChangeDay())
        {
            time = changingDayDelayTime;
            return;
        }
    }

    private bool TryDisplayMessage()
    {
        if (queue.Count == 0) return false;

        GameFlowEvent gameFlowEvent = queue.Dequeue();

        // UI에 추가
        var toast = PoolManager.Instance.Get<ToastMessage>("Prefabs/ToastMessage", toastParent, new Vector2(-384.0f, -position));
        toastParent.DOLocalMoveY(position, 0.125f, true).SetEase(Ease.OutQuad);
        toast.transform.DOLocalMoveX(0.0f, 0.25f).SetEase(Ease.OutQuad);
        position += 16.0f;
        toast.Initialize(gameFlowEvent.Message);

        return true;
    }

    private bool TryChangeDay()
    {
        if (isFading || remainDays == 0) return false;
        remainDays--;
        uiMain.ChangeOneDay();
        screenCover.DOFade(0.0f, 1.0f).OnComplete
        (() =>
            {
                isFading = false;
                if (remainDays == 0) uiMain.SetSkipButtonInteraction(true);
            }
        );
        isFading = true;

        return true;
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