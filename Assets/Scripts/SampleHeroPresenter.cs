using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SampleHeroPresenter : Poolable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveFrequency;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Vector2Int> moveCommand = new List<Vector2Int>();

    [SerializeField] private bool isMoving;
    [SerializeField] private float time;

    private Vector3 targetPosition;

    public void SetMoveCommand(List<Vector2Int> route)
    {
        moveCommand = route;
    }

    private void FixedUpdate()
    {
        spriteRenderer.sortingOrder = -(int)transform.localPosition.y;

        if (isMoving)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveSpeed * Time.fixedDeltaTime);
            if (Vector3.Distance(targetPosition, transform.localPosition) < 0.01f)
            {
                transform.localPosition = targetPosition;
                isMoving = false;
            }
        }
        else
        {
            if (moveCommand == null || moveCommand.Count == 0) return;

            if (time > 0)
            {
                time -= Time.fixedDeltaTime;
                return;
            }

            time = moveFrequency;
            targetPosition = transform.localPosition + (Vector3)(Vector2)moveCommand[0];
            moveCommand.RemoveAt(0);
            isMoving = true;
        }
    }
}