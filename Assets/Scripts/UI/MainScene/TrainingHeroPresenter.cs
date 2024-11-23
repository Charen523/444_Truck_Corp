using UnityEngine;
using UnityEngine.UI;

public class TrainingHeroPresenter : MonoBehaviour
{
    [SerializeField] private Image image; 
    [SerializeField] private CustomAnimator animator;

    public string Path;

    private void Start()
    {
        Initialize(Path);
    }

    public void Initialize(string path)
    {
        if (string.IsNullOrEmpty(path)) return;
        animator = new CustomAnimator(path, 6, false, false, null);
        animator.SetPlaying(true);
    }

    private void FixedUpdate()
    {
        if (animator == null) return;
        image.sprite = animator.GetSprite(Time.fixedDeltaTime);
    }
}