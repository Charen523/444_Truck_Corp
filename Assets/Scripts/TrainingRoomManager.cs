using System;
using UnityEngine;

public class TrainingRoomManager : Singleton<TrainingRoomManager>
{
    [SerializeField] private TrainingRoom[] rooms;

    public void OnRoomClicked(int number)
    {
        if (rooms[number].IsUsing)
        {
            // 캔슬 UI
        }
        else
        {
            // 캐릭터 선택 화면
        }
    }

    public void OnCancleButtonClicked(int number)
    {

    }

    public void On()
    {

    }

    private void FixedUpdate()
    {

    }
}

public class TrainingRoom
{
    public Character Character;
    public TimeSpan time;
    public bool IsUsing;

    public void AssignCharacter(Character character)
    {

    }

    public void CancleTraining()
    {

    }
}