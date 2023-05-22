using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon {

    public enum WeaponName {
        // Simple Melee
        HandAxe = 3,
        LightHammer = 4,
        // Martial Melee
        BattleAxe = 5,
        Longsword = 0,
        Shortsword = 1,
        WarHammer = 2
    }


    [System.Flags]
    public enum WeaponType {
        None = 0,
        SimpleMelee = 1 << 0,
        SimpleRanged = 1 << 1,
        MartialMelee = 1 << 2
    }

    [System.Flags]
    public enum WeaponProperty {
        None = 0,
        Versatile = 1 << 0,
        Finesse = 1 << 1,
        Light = 1 << 2,
        Heavy = 1 << 3,
        TwoHanded = 1 << 4,
        Thrown = 1 << 5
    }

    public string weaponTitle;
    public WeaponName weaponName;
    public WeaponType weaponType;
    public WeaponProperty weaponProperty;
    public int weaponCost;
    public int weaponWeight;
    public List<WeaponAttack> weaponAttacks;

    public Weapon (string weaponTitle, WeaponName weaponName, WeaponType weaponType, WeaponProperty weaponProperty, int weaponCost, int weaponWeight, List<WeaponAttack> weaponAttacks) {
        this.weaponTitle = weaponTitle;
        this.weaponName = weaponName;
        this.weaponType = weaponType;
        this.weaponProperty = weaponProperty;
        this.weaponCost = weaponCost;
        this.weaponWeight = weaponWeight;
        this.weaponAttacks = weaponAttacks;
    }
}






