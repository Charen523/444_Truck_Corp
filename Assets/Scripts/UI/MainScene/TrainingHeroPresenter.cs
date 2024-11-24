using System;
using UnityEngine;
using UnityEngine.UI;

public class TrainingHeroPresenter : MonoBehaviour
{
    [SerializeField] private Image image; 
    [SerializeField] private CustomAnimator animator;

    public string Path;

    private void Awake()
    {
        Initialize(Path, null);
    }

    public void Initialize(string path, Action action)
    {
        if (string.IsNullOrEmpty(path)) return;
        animator = new CustomAnimator(path, 10, false, false, action);
    }

    public void SetPlaying(bool value)
    {
        animator.SetPlaying(value);
    }

    public void SetOn(Action action)
    {
        animator.SetOnEndAnimation(action);
    }

    private void FixedUpdate()
    {
        if (animator == null) return;
        image.sprite = animator.GetSprite(Time.fixedDeltaTime);
        image.rectTransform.sizeDelta = new Vector2(image.sprite.rect.width * 1.5625f, image.sprite.rect.height * 1.5625f);
    }
}