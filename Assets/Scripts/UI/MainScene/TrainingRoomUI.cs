using TMPro;
using UnityEngine;

public class TrainingRoomUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject emptyUI;
    [SerializeField] private GameObject trainingUI;
    [SerializeField] private TrainingHeroPresenter heroPresenter;
    [SerializeField] private TrainingHeroPresenter dummyPresenter;
    [SerializeField] private TextMeshProUGUI dayText; // 일차 표시 텍스트

    [Header("External Popup")]
    [SerializeField] private PopupHeroSelect popupHeroSelect;

    [Header("Field")]
    [SerializeField] private bool isTraining;
    [SerializeField] private int RoomId;

    private int startedDay;
    private HeroData hero;

    private void OnEnable()
    {
        UpdateUIs();
    }

    private void UpdateUIs()
    {
        emptyUI.SetActive(!isTraining);
        trainingUI.SetActive(isTraining);
        if (isTraining)
        {
            dayText.text = $"{GameManager.Instance.Day - startedDay + 1}일차";
        }
        else
        {
            dayText.text = $"";
        }
    }

    public void OnClick()
    {
        if (isTraining)
        {
            GameManager.Instance.InvokeDialog
            (
                "훈련을 중지합니까?",
                (result) =>
                {
                    if (result == eDialogResult.Yes)
                    {
                        EndTraining();
                    }
                }
            );
        }
        else
        {
            popupHeroSelect.gameObject.SetActive(true);
            GameManager.Instance.HeroSelectAction = OnHeroSelected;
        }
    }

    private void OnHeroSelected(int idx)
    {
        GameManager.Instance.InvokeDialog
        (
            "훈련을 시작합니까?",
            (result) =>
            {
                if (result == eDialogResult.Yes)
                {
                    StartTraining(idx);
                }
            }
        );
    }

    public void StartTraining(int id)
    {
        hero = HeroManager.Instance.GetHero(id);
        string path = DataManager.Instance.GetCharacterSheetPath(hero.spriteIdx);
        heroPresenter.Initialize(path + "_Attack");
        startedDay = GameManager.Instance.Day;
        isTraining = true;
        UpdateUIs();
        HeroManager.Instance.AddTrainingSchedule(hero.id, startedDay);
    }

    public void EndTraining()
    {
        isTraining = false;
        UpdateUIs();

        // 계산
        int remainDay = hero.remainDay[RoomId];
        int dayDifference = remainDay + GameManager.Instance.Day - startedDay;
        int count = dayDifference / 5; // 5일당 3스탯씩 상승

        hero.remainDay[RoomId] = dayDifference - count * 5;

        if (RoomId == 0)
        {
            hero.status.STR += count * 3;
        }
        else if (RoomId == 1)
        {
            hero.status.DEX += count * 3;
        }
        else if (RoomId == 2)
        {
            hero.status.INT += count * 3;
        }
        else if (RoomId == 3)
        {
            hero.status.LUK += count * 3;
        }

        OpenStatUpPopup(remainDay, hero.remainDay[RoomId], count);
        HeroManager.Instance.CheckTrainingScheduleDone(hero.id);
        hero = null;
    }

    // 시작 경험치, 마지막 경험치, 스탯 업 횟수
    private void OpenStatUpPopup(int start, int end, int count)
    {
        GameManager.Instance.InvokeWarning($"{hero.name}의 {GetStatString()}이 {count * 3}만큼 상승했다!", "알림");
    }

    private string GetStatString()
    {
        if (RoomId == 0)
        {
            return "STR";
        }
        else if (RoomId == 1)
        {
            return "DEX";
        }
        else if (RoomId == 2)
        {
            return "INT";
        }
        else if (RoomId == 3)
        {
            return "LUK";
        }
        return "";
    }
}