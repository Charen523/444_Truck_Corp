using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : Poolable
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CustomContentSizeFitter sizeFitter;

    private float time;
    private bool isInitialized;

    public void Initialize(string message)
    {
        text.text = message;
        time = 20.0f;
        sizeFitter.UpdateFit();
        isInitialized = true;
        SetImageColorAlpha(1.0f);
        SetTextColorAlpha(1.0f);
    }

    private void SetImageColorAlpha(float value)
    {
        var color = image.color;
        color.a = 1.0f;
        image.color = color;
    }

    private void SetTextColorAlpha(float value)
    {
        var color = text.color;
        color.a = 1.0f;
        text.color = color;
    }

    private void FixedUpdate()
    {
        if (!isInitialized) return;

        if (time > 0.0f)
        {
            time -= Time.fixedDeltaTime;
            return;
        }

        var color = image.color;
        color.a = 0.0f;
        image.DOColor(color, 0.25f);

        color = text.color;
        color.a = 0.0f;
        text.DOColor(color, 0.25f).OnComplete
        (
            () =>
            {
                isInitialized = false;
                Pool.Return(this);
            }
        );
    }
}