using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;


[System.Serializable]
public class WeaponAction {

    private ActionType actionType;
    public string title;
    public DiceRoll.DiceType damageRoll;
    public DamageType damageType;
    /*public Ability.AbilityType attackRollModifier;*/
    public int range;

    public WeaponAction(ActionType actionType, string title, DiceRoll.DiceType damageRoll, DamageType damageType, /*Ability.AbilityType attackRollModifier,*/ int range) {
        this.actionType = actionType;
        this.title = title;
        this.damageRoll = damageRoll;
        this.damageType = damageType;
        /*this.attackRollModifier = attackRollModifier;*/
        this.range = range; 
    }
}
