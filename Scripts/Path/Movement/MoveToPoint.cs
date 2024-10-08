using UnityEngine;
using System.Collections;
using System;

public class MoveToPoint : MonoBehaviour {
    public Point targetPoint;
    public MovementStats movStat;
    public float stoppingDistance = 1f;
    public Vector3 offset = Vector3.zero;

    private bool isMoving = false;
    private Coroutine moveCoroutine;


    [ContextMenu("StartMoving")]
    public void StartMoving() {
        if (!isMoving && targetPoint != null) {
            isMoving = true;

            moveCoroutine = StartCoroutine(MoveTowardsPoint());
        }
    }

    public void StopMoving() {
        if (isMoving) {
            isMoving = false;
            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
            }
        }
    }


    public void SetTargetPoint(Point newTargetPoint) {
        targetPoint = newTargetPoint;
        if (isMoving) {
            StopMoving();
            StartMoving();
        }
    }

    public void SetOffset(Vector3 newOffset) {
        offset = newOffset;
    }

    private IEnumerator MoveTowardsPoint() {
        while (isMoving) {
            if (targetPoint == null) {
                yield break;
            }

            Vector3 targetPosition = targetPoint.transform.position + offset;
            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;

            if (distance < stoppingDistance) {
                Point nextPoint = targetPoint.GetNextPoint(this.gameObject);
                if (nextPoint != null) {
                    targetPoint = nextPoint;
                } else {
                    StopMoving();
                }
            } else {
                direction.Normalize();
                transform.position += direction * movStat.GetCurrentSpeed() * Time.deltaTime;
            }

            yield return null;
        }
    }

    public void SetStats(MovementStats movementStats) {
        movStat = ScriptableObject.CreateInstance<MovementStats>();
        movStat.Init(movementStats);
    }
}
