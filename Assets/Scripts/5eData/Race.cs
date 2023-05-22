using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Race {
    public enum RaceType {
        Human,
        Elf,
        Dwarf
    }

    public enum Size {
        Tiny,
        Small,
        Medium,
        Large,
        Huge
    }

    public RaceType raceType;
    public Size size;
    public int speed;
    public List<Sense> senses;

    public List<AbilityScoreIncrease> abilityScoreIncrease;
    public List<Skill.SkillType> skillProficiencies;
    public List<Tool.ToolProficiency> toolProficiencies;
    public List<Language.LanguageType> languages;
    public List<Weapon.WeaponName> weaponProficiencies;
    public List<Feature> features;



    public Race(
        RaceType raceType, 
        Size size, 
        int speed, 
        List<Sense> senses, 
        List<AbilityScoreIncrease> abilityScoreIncrease, 
        List<Skill.SkillType> skillProficiencies,
        List<Tool.ToolProficiency> toolProficiencies,
        List<Language.LanguageType> languages,
        List<Weapon.WeaponName> weaponProficiencies,
        List<Feature> features) {
        this.raceType = raceType;
        this.size = size;
        this.speed = speed;
        this.senses = senses;
        this.abilityScoreIncrease = abilityScoreIncrease;
        this.skillProficiencies = skillProficiencies;
        this.toolProficiencies = toolProficiencies;
        this.languages = languages;
        this.weaponProficiencies = weaponProficiencies;
        this.features = features;
        
    }
}