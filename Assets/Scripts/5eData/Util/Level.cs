using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {
    public int level;
    public int experienceToNextLevel;
    public int totalExperienceToNextLevel;
    public int proficiencyBonus;

    public Level(int level, int experienceToNextLevel, int totalExperienceToNextLevel, int proficiencyBonus) {
        this.level = level;
        this.experienceToNextLevel = experienceToNextLevel;
        this.totalExperienceToNextLevel = totalExperienceToNextLevel;
        this.proficiencyBonus = proficiencyBonus;
    }
}
