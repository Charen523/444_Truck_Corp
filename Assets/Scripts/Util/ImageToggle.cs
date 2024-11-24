using UnityEngine;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour
{
    [Header("Images")]
    public Sprite imageOn; 
    public Sprite imageOff;

    [Header("Components")]
    public Image targetImage; 

    [Header("State")]
    public bool isOn = false;

    private Button button; 

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        button.onClick.AddListener(Toggle);

        if (targetImage == null)
            targetImage = GetComponent<Image>();

        UpdateImage(); 
    }

    public void Toggle()
    {
        isOn = !isOn; 
        UpdateImage();
    }

    private void UpdateImage()
    {
        if (targetImage != null)
        {
            targetImage.sprite = isOn ? imageOn : imageOff; 
        }
    }
}
