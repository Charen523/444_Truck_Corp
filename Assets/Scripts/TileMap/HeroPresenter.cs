using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HeroPresenter : Poolable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float movementFrequency;
    [SerializeField] private float movementSpeed;
    [SerializeField] private List<Vector2Int> moveCommand;

    [SerializeField] private bool isMoving;
    [SerializeField] private float time;
    [SerializeField] private string texturePath;
    [SerializeField] private CustomAnimator animator;

    private bool[,] tiles;
    private Action onMoveComplete;

    public Vector2Int Position
    {
        get
        {
            return new Vector2Int((int)targetPosition.x, (int)-targetPosition.y);
        }
    }

    private float movementProgress;
    private Vector3 lastPosition;
    private Vector3 targetPosition;
    private DirectionType direction;

    public void Initialize(string texturePath, bool[,] tiles)
    {
        this.texturePath = texturePath;
        this.tiles = tiles;

        animator = new CustomAnimator(texturePath, 9, true, true, null);
    }

    public void SetMoveCommand(List<Vector2Int> route, Action onMoveComplete = null)
    {
        moveCommand = route;
        time = movementFrequency;
        this.onMoveComplete = onMoveComplete;
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
                movementProgress = 0.0f;
                isMoving = false;
                if (moveCommand != null && moveCommand.Count == 0) onMoveComplete?.Invoke();
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
            else
            {
                UpdateAnimation();
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
        animator.SetPlaying(isMoving);
        spriteRenderer.sprite = animator.GetSprite(Time.fixedDeltaTime);
    }
}