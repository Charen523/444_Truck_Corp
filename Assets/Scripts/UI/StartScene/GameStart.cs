using System.Collections;
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
}
