using System;
using TMPro;
using UnityEngine;

public class TrainingRoomUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject emptyUI;
    [SerializeField] private GameObject trainingUI;
    [SerializeField] private TrainingHeroPresenter heroPresenter;
    [SerializeField] private TrainingHeroPresenter dummyPresenter;
    [SerializeField] private TrainingHeroPresenter dummyDamageVFXPresenter;
    [SerializeField] private TextMeshProUGUI dayText; // 일차 표시 텍스트

    [Header("External Popup")]
    [SerializeField] private PopupHeroSelect popupHeroSelect;

    [Header("Field")]
    [SerializeField] private bool isTraining;
    [SerializeField] private int RoomId;

    private int startedDay;
    private HeroData hero;

    private readonly (string StatName, Action<Status, int> IncreaseMethod)[] statusTuple =
    {
        ("STR", (Status status, int value) => { status.STR += value; }),
        ("DEX", (Status status, int value) => { status.DEX += value; }),
        ("INT", (Status status, int value) => { status.INT += value; }),
        ("LUK", (Status status, int value) => { status.LUK += value; }),
    };

    private void OnEnable()
    {
        UpdateUIs();
    }

    private void UpdateUIs()
    {
        emptyUI.SetActive(!isTraining);
        trainingUI.SetActive(isTraining);
        dayText.text = (isTraining) ? $"{GameManager.Instance.Day - startedDay + 1}일차" : "";
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
        startedDay = GameManager.Instance.Day;
        isTraining = true;
        UpdateUIs();

        hero = HeroManager.Instance.GetHero(id);
        string heroPath = DataManager.Instance.GetCharacterSheetPath(hero.spriteIdx);
        string vfxPath = DataManager.Instance.GetDamageVFXPath(hero.spriteIdx);
        heroPresenter.Initialize(heroPath + "_Attack", () => dummyPresenter.SetPlaying(true));
        dummyDamageVFXPresenter.Initialize(vfxPath, null);
        heroPresenter.SetOn(() => { dummyPresenter.SetPlaying(true); dummyDamageVFXPresenter.SetPlaying(true); });
        dummyPresenter.SetOn(() => heroPresenter.SetPlaying(true));
        heroPresenter.SetPlaying(true);

        HeroManager.Instance.AddTrainingSchedule(hero.id);
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
        statusTuple[RoomId].IncreaseMethod(hero.status, count * 3);

        OpenStatUpPopup(remainDay, hero.remainDay[RoomId], count);
        HeroManager.Instance.CheckTrainingScheduleDone(hero.id);
        hero = null;
    }

    // 시작 경험치, 마지막 경험치, 스탯 업 횟수
    private void OpenStatUpPopup(int start, int end, int count)
    {
        if (count == 0)
        {
            GameManager.Instance.InvokeWarning($"훈련을 중지했습니다.", "알림");
        }
        else
        {
            GameManager.Instance.InvokeWarning($"{hero.name}의 {statusTuple[RoomId].StatName}이 {count * 3}만큼 상승했다!", "알림");
        }
    }
}