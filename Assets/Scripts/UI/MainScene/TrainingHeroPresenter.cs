using UnityEngine;
using UnityEngine.UI;

public class TrainingHeroPresenter : MonoBehaviour
{
    [SerializeField] private Image image; 
    [SerializeField] private CustomAnimator animator;
    public string Path;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        animator = new CustomAnimator(Path, 6, false);
        animator.SetPlaying(true);
    }

    private void FixedUpdate()
    {
        image.sprite = animator.GetSprite(Time.fixedDeltaTime);
    }
}