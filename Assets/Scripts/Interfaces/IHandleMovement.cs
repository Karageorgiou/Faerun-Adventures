using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandleMovement {
    void HandleMovement();

    bool IsWalking();

    public void SetTargetPosition(Vector3 targetPosition, Action onTargetPositionReached);
}
