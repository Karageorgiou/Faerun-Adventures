using System.Collections;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;

[System.Serializable]
public class WeaponAttack {

    public string title;
    public DiceRoll.DiceType damageRoll;
    /*public Ability.AbilityType attackRollModifier;*/
    public int attackRange;
    public DamageType damageType;


}