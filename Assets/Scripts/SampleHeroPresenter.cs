using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SampleHeroPresenter : Poolable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float movementFrequency;
    [SerializeField] private float movementSpeed;
    [SerializeField] private List<Vector2Int> moveCommand = new List<Vector2Int>();

    [SerializeField] private bool isMoving;
    [SerializeField] private float time;
    [SerializeField] private string texturePath;
    [SerializeField] private Texture2D texture;
    [SerializeField] private CustomAnimator animator;

    private Vector3 lastPosition;
    private Vector3 targetPosition;
    private float movementProgress;
    private DirectionType direction;

    private void Start()
    {
        animator = new CustomAnimator(texturePath, 9, 12);
    }

    public void SetMoveCommand(List<Vector2Int> route)
    {
        moveCommand = route;
        time = movementFrequency;
    }

    private void FixedUpdate()
    {
        spriteRenderer.sortingOrder = -(int)transform.localPosition.y;

        if (isMoving)
        {
            movementProgress += movementSpeed * Time.fixedDeltaTime;
            transform.localPosition = Vector3.Lerp(lastPosition, targetPosition, movementProgress);
            UpdateAnimation();
            if (movementProgress > 1.0f)
            {
                transform.localPosition = targetPosition;
                isMoving = false;
            }
        }
        else
        {
            if (moveCommand != null && moveCommand.Count > 0)
            {
                if (time > 0)
                {
                    time -= Time.fixedDeltaTime;
                    return;
                }
                else
                {
                    time = movementFrequency;
                    movementProgress = 0;
                    lastPosition = transform.localPosition;
                    Vector2Int delta = moveCommand[0];
                    animator.SetDirection(directionDictionary[delta]);
                    targetPosition = transform.localPosition + (Vector3)(Vector2)delta;
                    moveCommand.RemoveAt(0);
                    isMoving = true;
                }
            }
        }
    }

    private static readonly Dictionary<Vector2Int, DirectionType> directionDictionary = new Dictionary<Vector2Int, DirectionType>()
    {
        {new Vector2Int(0, +1), DirectionType.Up},
        {new Vector2Int(0, -1), DirectionType.Down},
        {new Vector2Int(-1, 0), DirectionType.Left},
        {new Vector2Int(+1, 0), DirectionType.Right},
    };

    private void UpdateAnimation()
    {
        spriteRenderer.sprite = animator.GetSprite(Time.fixedDeltaTime);
    }
}

[Serializable]
public class CustomAnimator
{
    private static readonly Dictionary<DirectionType, int> directionDictionary = new Dictionary<DirectionType, int>()
    {
        { DirectionType.Up, 0 },
        { DirectionType.Left, 1 },
        { DirectionType.Down, 2 },
        { DirectionType.Right, 3 },
    };

    private int direction;
    public int CurrentFrame;
    public int FrameSpeed;
    private float frameDuration;
    private float framePerSecond;
    private float currentDuration;
    private bool isMoving;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int maxFrame;

    public CustomAnimator(string path, int maxFrame, int framePerSecond)
    {
        sprites = Resources.LoadAll<Sprite>(path);
        frameDuration = 1.0f / framePerSecond;
        this.framePerSecond = framePerSecond;
        this.maxFrame = maxFrame;
        Debug.Log($"{path}의 스프라이트 개수 : {sprites.Length}");
    }

    public void SetDirection(DirectionType directionType)
    {
        direction = directionDictionary[directionType];
    }

    public Sprite GetSprite(float deltaTime)
    {
        currentDuration += deltaTime;
        int addFrame = (int)(currentDuration * framePerSecond);
        if (addFrame > 0)
        {
            CurrentFrame += addFrame;
            CurrentFrame %= maxFrame;
            currentDuration -= addFrame * frameDuration;
        }
        CurrentFrame = (isMoving) ? CurrentFrame : 0; 
        return sprites[direction * maxFrame + CurrentFrame];
    }
}