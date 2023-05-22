using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewWeaponData", menuName = "ScriptableObjects/Weapon Data")]
public class WeaponDataSO : ScriptableObject {
    public List<Weapon> weapons;

    private void Reset() {
        weapons = new List<Weapon>();
    }
}
