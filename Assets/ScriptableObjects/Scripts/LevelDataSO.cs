using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/Level Data")]
public class LevelDataSO : ScriptableObject {
    public int maxLevel;
    public List<Level> levels;

    private void Reset() {
        maxLevel = 0;
        levels = new List<Level>();
    }

    [ContextMenu("Initialize Level Data")]

    private void InitializeLevelData() {
        maxLevel = 20;
        levels = new List<Level>();
        levels = FillLevelsList(maxLevel);
    }

    private List<Level> FillLevelsList(int maxLevel) {
        List<Level> filledLevels = new List<Level>();

        
        for (int level = 1; level < maxLevel; level++) {
            int totalExperience = GetTotalExperienceToNextLevel(level);
            int experienceToNextLevel = totalExperience - GetTotalExperienceToNextLevel(level - 1);

            int proficiencyBonus = 0;
            if (level > 0 && level <= 4) {
                proficiencyBonus = 2;
            }
            if (level > 4 && level <= 8) {
                proficiencyBonus = 3;
            }
            if (level > 8 && level <= 12) {
                proficiencyBonus = 4;
            }
            if (level > 12 && level <= 16) {
                proficiencyBonus = 5;
            }
            if (level > 16 && level <= 20) {
                proficiencyBonus = 6;
            }


            Level levelData = new Level(level, experienceToNextLevel, totalExperience, proficiencyBonus);

            filledLevels.Add(levelData);
        }
         
        return filledLevels;
    }

    private int GetTotalExperienceToNextLevel(int level) {
        // Calculate the experience required for each level based on the provided pairs
        switch (level) {
            case 1:
                return 300;
            case 2:
                return 900;
            case 3:
                return 2700;
            case 4:
                return 6500;
            case 5:
                return 14000;
            case 6:
                return 23000;
            case 7:
                return 34000;
            case 8:
                return 48000;
            case 9:
                return 64000;
            case 10:
                return 85000;
            case 11:
                return 100000;
            case 12:
                return 120000;
            case 13:
                return 140000;
            case 14:
                return 165000;
            case 15:
                return 195000;
            case 16:
                return 225000;
            case 17:
                return 265000;
            case 18:
                return 305000;
            case 19:
                return 355000;
            default:
                return 0;
        }
    }
}
