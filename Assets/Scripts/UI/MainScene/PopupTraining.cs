using UnityEngine;

public class PopupTraining : MonoBehaviour
{
    [SerializeField] private PopupHeroSelect popupHeroSelect;

    private TrainingRoom[] trainingRooms;
    private int selectedRoom;

    private void Awake()
    {
        trainingRooms = new TrainingRoom[4];
        selectedRoom = -1;
    }

    public void OnClickRoom(int number)
    {
        // 현재 트레이닝 중일 경우
        if (trainingRooms[number].IsTraining)
        {

        }
        selectedRoom = number;
    }

    public void OnEnable()
    {
        GameManager.Instance.HeroSelectAction -= OnHeroSelected;
        GameManager.Instance.HeroSelectAction += OnHeroSelected;
    }

    public void OnDisable()
    {
        GameManager.Instance.HeroSelectAction -= OnHeroSelected;
    }

    private void OnHeroSelected(int idx)
    {

    }
}

public class TrainingRoom : MonoBehaviour
{
    public bool IsTraining;
    private HeroData hero;
    private TrainingData trainingData;
    private int leftDay;

    public void OnTrainingStart(HeroData hero, TrainingData trainingData)
    {
        this.hero = hero;
        this.trainingData = trainingData;


    }
}