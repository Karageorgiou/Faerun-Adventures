using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable {
    public Vector3 GetWorldPosition();

    public int GetArmorClass();

    public void ReceiveAttack(bool attackHit, int damage);
}
