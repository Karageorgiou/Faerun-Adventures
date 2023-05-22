using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Background {
    public enum BackgroundType { // A to C, will add more later
        Acolyte,
        Charlatan,
        CityWatch,
        Criminal,
        Soldier,
        None
    }

    public BackgroundType backgroundType;
    public List<Skill.SkillType> skillProficiencies;
    public List<Tool.ToolProficiency> toolProficiencies;
    public List<Language.LanguageType> languages;
    public List<Feature> features;
    //todo: equipment
    

    public Background(
        BackgroundType backgroundType,
        List<Skill.SkillType> skillProficiencies,
        List<Tool.ToolProficiency> toolProficiencies,
        List<Language.LanguageType> languages,
        List<Feature> features) {
        this.backgroundType = backgroundType;
        this.skillProficiencies = skillProficiencies;
        this.toolProficiencies = toolProficiencies;
        this.languages = languages;
        this.features = features;
    }
}