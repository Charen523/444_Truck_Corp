using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartBtn : MonoBehaviour
{
    public void OnBtnClicked()
    {
        SceneManager.LoadScene(1);
    }
}
