using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : Poolable
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    private float time;
    private bool isInitialized;

    public void Initialize(string message)
    {
        text.text = message;
        time = 5.0f;
        isInitialized = true;
    }

    private void FixedUpdate()
    {
        if (!isInitialized) return;

        if (time > 0.0f)
        {
            time -= Time.fixedDeltaTime;
        }
        isInitialized = false;
        Pool.Return(this);
    }
}