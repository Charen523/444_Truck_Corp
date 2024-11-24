using UnityEngine;

public class GameFlowDisplay : MonoBehaviour
{
    private int remainDays;


    public void OnDayChanged(int delta)
    {
        // 화면 페이드 아웃
        // 
        remainDays = delta;

        //
    }

    private float updateTime;

    private void FixedUpdate()
    {
        if (updateTime > 0)
        {
            updateTime -= Time.deltaTime;
            return;
        }
        // 일 수 차감 및 자원 차감
        // 이벤트가 있다면

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

}