using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewWeaponInventoryData", menuName = "ScriptableObjects/Weapon Inventory Data")]
public class WeaponInventoryDataSO : ScriptableObject {
    [SerializeField] WeaponDataSO weaponData;

    public List<Weapon> weaponInventory;



    [ContextMenu("Receive Longsword")]
    private void ReceiveLongsword() {
        Debug.Log((int)Weapon.WeaponName.Longsword);

        Weapon newLongsword = new Weapon(
            weaponData.weapons[(int)Weapon.WeaponName.Longsword].weaponTitle,
            weaponData.weapons[(int)Weapon.WeaponName.Longsword].weaponName,
            weaponData.weapons[(int)Weapon.WeaponName.Longsword].weaponType,
            weaponData.weapons[(int)Weapon.WeaponName.Longsword].weaponProperty,
            weaponData.weapons[(int)Weapon.WeaponName.Longsword].weaponCost,
            weaponData.weapons[(int)Weapon.WeaponName.Longsword].weaponWeight,
            weaponData.weapons[(int)Weapon.WeaponName.Longsword].weaponAttacks);
        weaponInventory.Add(newLongsword);
    }

}
