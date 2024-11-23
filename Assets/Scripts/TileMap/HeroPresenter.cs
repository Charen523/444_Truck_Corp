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

    private Action onMoveComplete;

    public Vector2Int Position
    {
        get
        {
            return new Vector2Int((int)targetPosition.x, (int)-targetPosition.y);
        }
    }

    private float movementProgress;
    private DirectionType direction;
    private Vector3 lastPosition;
    private Vector3 targetPosition;
    private EventLocation targetLocation;

    public void Initialize(string texturePath)
    {
        this.texturePath = texturePath;

        animator = new CustomAnimator(texturePath, 9, true, true, null);
        direction = DirectionType.Down;
        targetPosition = transform.localPosition;
        UpdateAnimation();
    }

    public void SetMoveCommand(List<Vector2Int> route, Action onMoveComplete = null)
    {
        if (targetLocation != null)
        {
            TileMapManager.Instance.ReturnLocation(targetLocation);
            targetLocation = null;
        }
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
            if ((moveCommand == null || moveCommand.Count == 0) && targetLocation == null)
            {
                FindNewTargetLocation();
            }
            else if (moveCommand != null && moveCommand.Count > 0)
            {
                UpdateStep();
            }
            else
            {
                animator.SetPlaying(false);
            }
        }
        UpdateAnimation();
    }

    private void UpdateStep()
    {
        if (time > 0)
        {
            time -= Time.fixedDeltaTime;
            return;
        }

        time = movementFrequency;
        movementProgress = 0;
        lastPosition = transform.localPosition;
        direction = directionDictionary[moveCommand[0]];
        targetPosition = transform.localPosition + (Vector3)(Vector2)moveCommand[0];
        moveCommand.RemoveAt(0);
        isMoving = true;
        animator.SetPlaying(true);
    }

    private void FindNewTargetLocation()
    {
        var target = TileMapManager.Instance.GetEmptyLocation();
        if (target != null)
        {
            Vector2Int position = new Vector2Int((int)target.transform.localPosition.x, -(int)target.transform.localPosition.y);
            var route = TileMapManager.Instance.GetRoute(Position, position);
            SetMoveCommand(route, OnMoveToTargetLocationComplete);
            targetLocation = target;
        }
    }

    private void OnMoveToTargetLocationComplete()
    {
        direction = targetLocation.Direction;
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
        animator.SetDirection(direction);
        spriteRenderer.sprite = animator.GetSprite(Time.fixedDeltaTime);
    }
}