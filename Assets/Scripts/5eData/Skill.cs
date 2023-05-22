using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill {
    public string skillTitle;
    public enum SkillType {
        Acrobatics,
        AnimalHandling,
        Arcana,
        Athletics,
        Deception,
        History,
        Insight,
        Intimidation,
        Investigation,
        Medicine,
        Nature,
        Perception,
        Performance,
        Persuasion,
        Religion,
        SleightOfHand,
        Stealth,
        Survival
    }

    private CharacterDataSO CharacterDataSO {
        get;
        set;
    }

     
    public SkillType skillType;
    public Ability.AbilityType abilityType;
    public bool proficiency;
    public int SkillModifier {
        get {
            int mod = CharacterDataSO.GetAbilityModifier(abilityType);
            if (proficiency) {
                mod += CharacterDataSO.proficiencyBonus;
            }
            return mod;
        }
    }

   
    public Skill(CharacterDataSO characterDataSO,SkillType skillType, Ability.AbilityType abilityType, bool proficiency) {
        CharacterDataSO = characterDataSO;
        this.skillType = skillType;
        this.abilityType = abilityType;
        this.proficiency = proficiency;
    }

    public int GetSkillModifier() {
        int mod = CharacterDataSO.GetAbilityModifier(abilityType);
        if (proficiency) {
            mod += CharacterDataSO.proficiencyBonus;
        }
        return mod;
    }

}
