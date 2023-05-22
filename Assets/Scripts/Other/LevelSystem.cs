using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem {
    public int level;
    public int experience;
    public int[] experienceToNextLevel;
    private int MaxLevel;

    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;


    public LevelSystem() {
        level = 0;
        experience = 0;
        MaxLevel = 20;
        experienceToNextLevel = new int[MaxLevel];

        for (int i = 1; i < MaxLevel; i++) {
            experienceToNextLevel[i] = CalculateExperienceForLevel(i);
            
        }
    }

    private int CalculateExperienceForLevel(int level) {
        // Based on 5th Edition SRD
        if (level <= 0) {
            return 0;
        } else if (level >= 1 && level <= 4) {
            return (level - 1) * 300;
        } else if (level >= 5 && level <= 10) {
            return (level - 1) * 600;
        } else if (level >= 11 && level <= 16) {
            return (level - 1) * 1200;
        } else if (level >= 17) {
            return (level - 1) * 2000;
        }

        return 0; 
    }

    // PUBLIC METHODS

    public void AddExperience(int amount) {
        experience += amount;
        if (experience >= experienceToNextLevel[level]) {
            experience -= experienceToNextLevel[level];
            level++;
            if (OnLevelChanged != null) {
                OnLevelChanged(this, EventArgs.Empty);
            }
        }
        if ( OnExperienceChanged != null ) { 
            OnExperienceChanged(this, EventArgs.Empty);
        }
    }

    public int GetLevel() {
        return level;
    }
}
