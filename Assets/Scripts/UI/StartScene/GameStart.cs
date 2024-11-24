using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        StartCoroutine(DelayedSceneChange(1f));
    }

    private IEnumerator DelayedSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit(); // 실제 빌드에서 애플리케이션 종료
#endif
    }
}
