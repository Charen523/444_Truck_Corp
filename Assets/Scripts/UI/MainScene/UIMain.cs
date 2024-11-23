using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private TextMesh goldText;
    [SerializeField] private TextMesh timeText;


    private void Start()
    {
        // AudioManager에 Slider 등록
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterSlider(volumeSlider);
        }
    }

}