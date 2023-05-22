using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IAttacker {

    public Vector3 GetWorldPosition();
    public void Attack(IAttackable target, Action onAttackComplete);
}
