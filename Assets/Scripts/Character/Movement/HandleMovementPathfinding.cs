using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMovementPathfinding : MonoBehaviour, IHandleMovement {
    [SerializeField] private float movementSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;

    private List<Vector3> pathVectorList;
    private int currentPathIndex;
    private bool isWalking;
    private bool isMoving;

    private Vector3 targetPosition;

    private Vector2 inputVector;

    private void Start() {
        inputVector = new Vector2();
    }



    public void HandleMovement() {
        if (pathVectorList != null) {
            isWalking = true;
            if (!isMoving) {
                if (currentPathIndex < pathVectorList.Count) {
                    targetPosition = pathVectorList[currentPathIndex];
                    currentPathIndex++;
                    isMoving = true;
                } else {
                    StopMoving();
                    return;
                }
            }

            Vector3 moveDir = (targetPosition - transform.position).normalized;
            Vector3 movementDirection = new Vector3(moveDir.x, 0, moveDir.z);
            float movementDistance = movementSpeed * Time.deltaTime;

            if (movementDistance >= Vector3.Distance(transform.position, targetPosition)) {
                transform.position = targetPosition;
                isMoving = false;

                if (currentPathIndex >= pathVectorList.Count) {
                    StopMoving();
                    //Debug.Log("Reached Final Position");
                }
            } else {
                transform.position += movementDirection * movementDistance;
                transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotationSpeed);
            }
        } else {
            // No path to target
        }
    }





    public Vector3 GetPosition() {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition, Action onReachedTargetPosition) {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.GetInstance().FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1) {
            pathVectorList.RemoveAt(0);
        }

        StartCoroutine(WaitForTargetPositionReached(targetPosition, onReachedTargetPosition));
    }

    public void SetTargetPositionForAttack(Vector3 targetPosition, Action onReachedTargetPosition) {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.GetInstance().FindPathForAttack(GetPosition(), targetPosition);
        

        if (pathVectorList != null && pathVectorList.Count > 1) {
            pathVectorList.RemoveAt(pathVectorList.Count - 1); // Remove last path step
            pathVectorList.RemoveAt(0);
        }

        StartCoroutine(WaitForTargetPositionReached(targetPosition, onReachedTargetPosition));
    }

    private IEnumerator WaitForTargetPositionReached(Vector3 targetPosition, Action onReachedTargetPosition) {
        while (pathVectorList != null && pathVectorList.Count > 0) {
            yield return null;
        }
        // Target position reached, invoke the callback
        onReachedTargetPosition?.Invoke();
    }

    private void StopMoving() {
        pathVectorList = null;
        inputVector = Vector3.zero;
        isWalking = false;
    }

    public bool IsWalking() {
        return isWalking;
    }
}
