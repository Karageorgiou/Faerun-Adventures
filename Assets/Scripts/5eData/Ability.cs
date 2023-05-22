using System;
using UnityEditor;
using UnityEngine;
using static Skill;

[System.Serializable]
public class Ability {
    public enum AbilityType {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }


    /*[Flags]
    public enum AbilityType {
        Strength = 1 << 0,
        Dexterity = 1 << 1,
        Constitution = 1 << 2,
        Intelligence = 1 << 3,
        Wisdom = 1 << 4,
        Charisma = 1 << 5
    }
*/

    public AbilityType type;
    public int abilityScore;
    public bool proficiency;
    public int Modifier {
        get {
            return CalculateModifier();
        }
    }



    public Ability(AbilityType abilityType, int abilityScore, bool proficiency) {
        this.type = abilityType;
        this.abilityScore = abilityScore;
        this.proficiency = proficiency;
    }


    public void IncreaseScore() {
        this.abilityScore += 1;
    }

    public void DecreaseScore() {
        this.abilityScore -= 1;
    } 

    private int CalculateModifier() {
        int modifier = this.abilityScore - 10;
        modifier += modifier < 0 ? -1 : 0;
        modifier /= 2;
        //Debug.Log(type + " Ability: +" + modifier);
        return modifier;
    }
}

