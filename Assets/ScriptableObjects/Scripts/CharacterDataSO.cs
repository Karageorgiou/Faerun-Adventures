using DnD5eData;
using System;
using System.Collections.Generic;
using GameUtils;
using UnityEditor;
using UnityEngine;
using static Skill;

[System.Serializable]
[CreateAssetMenu(fileName = "NewCharacterData", menuName = "ScriptableObjects/Character Data")]
public class CharacterDataSO : ScriptableObject {
    [SerializeField] public bool isInitialized;
    [SerializeField] public bool receivedProperties;
    
    /** READ ONLY! */
    /** */[SerializeField] public RaceDataSO raceData;
    /** */[SerializeField] public SkillDataSO skillData;
    /** */[SerializeField] public LevelDataSO levelData;
    /** */[SerializeField] public BackgroundDataSO backgroundData;
    /**------------*/


    [SerializeField] public WeaponInventoryDataSO weaponInventoryData;

    

    [Header("Main Info")]
    public string characterFullName;
    public Race.RaceType characterRace;
    public Classes.Class characterClass;
    public int level;
    public int currentExperience;
    public int experienceToNextLevel;
    public int totalExperienceToNextLevel;
    public Alignments.Alignment alignment;
    public Background.BackgroundType background;
    public bool inspiration;

    [Header("Main Stats")]
    public int proficiencyBonus;
    public int initiative;
    public int armorClass;
    public int speed;
    public Race.Size size;
    public int hitPoints;
    public int temporaryHitPoints;
    public List<Sense> senses;
    [Space]

    [Header("Abilities & Skills")]
    public List<Ability> abilities;
    public List<Skill> skills;
    [Space]

    [Header("Proficiencies")]
    public List<Tool.ToolProficiency> toolProficiencies;
    public List<Weapon.WeaponName> weaponProficiencies;
    [Space]

    public List<Language.LanguageType> languages;
    public List<Feature> features;
    [Space]

    [Header("Inventory")]
    public List<Weapon> weapons;

    [Header("Actions")]
    [Header("Weapon  Actions")]
    public List<WeaponAction> weaponActions;



    public void Init(CharacterDataSO characterDataSO) {
        this.isInitialized = characterDataSO.isInitialized;
        this.receivedProperties = characterDataSO.receivedProperties;
        this.raceData = characterDataSO.raceData ?? throw new ArgumentNullException(nameof(raceData));
        this.skillData = characterDataSO.skillData ?? throw new ArgumentNullException(nameof(skillData));
        this.levelData = characterDataSO.levelData ?? throw new ArgumentNullException(nameof(levelData));
        this.backgroundData = characterDataSO.backgroundData ?? throw new ArgumentNullException(nameof(backgroundData));
        this.weaponInventoryData = characterDataSO.weaponInventoryData ?? throw new ArgumentNullException(nameof(weaponInventoryData));
        this.characterFullName = characterDataSO.characterFullName ?? throw new ArgumentNullException(nameof(name));
        this.characterRace = characterDataSO.characterRace;
        this.characterClass = characterDataSO.characterClass;
        this.level = characterDataSO.level;
        this.currentExperience = characterDataSO.currentExperience;
        this.experienceToNextLevel = characterDataSO.experienceToNextLevel;
        this.totalExperienceToNextLevel = characterDataSO.totalExperienceToNextLevel;
        this.alignment = characterDataSO.alignment;
        this.background = characterDataSO.background;
        this.inspiration = characterDataSO.inspiration;
        this.proficiencyBonus = characterDataSO.proficiencyBonus;
        this.initiative = characterDataSO.initiative;
        this.armorClass = characterDataSO.armorClass;
        this.speed = characterDataSO.speed;
        this.size = characterDataSO.size;
        this.hitPoints = characterDataSO.hitPoints;
        this.temporaryHitPoints = characterDataSO.temporaryHitPoints;
        this.senses = characterDataSO.senses ?? throw new ArgumentNullException(nameof(senses));
        this.abilities = characterDataSO.abilities ?? throw new ArgumentNullException(nameof(abilities));
        this.skills = characterDataSO.skills ?? throw new ArgumentNullException(nameof(skills));
        this.toolProficiencies = characterDataSO.toolProficiencies ?? throw new ArgumentNullException(nameof(toolProficiencies));
        this.weaponProficiencies = characterDataSO.weaponProficiencies ?? throw new ArgumentNullException(nameof(weaponProficiencies));
        this.languages = characterDataSO.languages ?? throw new ArgumentNullException(nameof(languages));
        this.features = characterDataSO.features ?? throw new ArgumentNullException(nameof(features));
        this.weapons = characterDataSO.weapons ?? throw new ArgumentNullException(nameof(weapons));
    }






    // INIT METHODS

    private void InitializeLevel() {
        Level tempLevel = new Level(
            levelData.levels[0].level, 
            levelData.levels[0].experienceToNextLevel, 
            levelData.levels[0].totalExperienceToNextLevel,
            levelData.levels[0].proficiencyBonus);
        level = tempLevel.level;
        experienceToNextLevel = tempLevel.experienceToNextLevel;
        totalExperienceToNextLevel = tempLevel.totalExperienceToNextLevel;
        currentExperience = 0;
        proficiencyBonus = tempLevel.proficiencyBonus;

    }

    private void InitializeAbilities() {
        abilities = new List<Ability> {
            new Ability(Ability.AbilityType.Strength, 10, false),
            new Ability(Ability.AbilityType.Dexterity, 10, false),
            new Ability(Ability.AbilityType.Constitution, 10, false),
            new Ability(Ability.AbilityType.Intelligence, 10, false),
            new Ability(Ability.AbilityType.Wisdom, 10, false),
            new Ability(Ability.AbilityType.Charisma, 10, false)
        };
    }

    private void InitializeSkills() {
        skills = new List<Skill>();
        for (int i = 0; i < skillData.skills.Count; i++) {
            Skill skill = skillData.skills[i];
            skills.Add(new Skill(this,skill.skillType,skill.abilityType,skill.proficiency));
        }
    }

    private void InitializeToolProficiencies() {
        toolProficiencies = new List<Tool.ToolProficiency>();
    }

    private void InitializeLanguages() {
        languages = new List<Language.LanguageType>();
    }

    private void InitializeFeatures() {
        features = new List<Feature>();
    }

    private void InitializeSenses() {
        senses = new List<Sense>();
    }

    private void InitializeWeapons() {
        weapons = new List<Weapon>();
    }

    private void InitializeWeaponActions() {
        weaponActions = new List<WeaponAction>();
    }

    private void ReceiveBackgroundData() {
        if (background != Background.BackgroundType.None) {
            if (backgroundData != null) {
                Background tempBackground = new Background(
                    backgroundData.backgrounds[(int)background].backgroundType,
                    backgroundData.backgrounds[(int)background].skillProficiencies,
                    backgroundData.backgrounds[(int)background].toolProficiencies,
                    backgroundData.backgrounds[(int)background].languages,
                    backgroundData.backgrounds[(int)background].features);
                if (tempBackground != null) {
                    Debug.Log("Received background: " + tempBackground.backgroundType);
                    foreach (Skill.SkillType skillType in tempBackground.skillProficiencies) {
                        Debug.Log("Received skillProficiencies: " + skillType);
                        skills[(int)skillType].proficiency = true;
                    }
                    foreach (Tool.ToolProficiency skillType in tempBackground.toolProficiencies) {
                        toolProficiencies.Add(skillType);
                        Debug.Log("Received toolProficiencies: " + skillType);
                    }
                    foreach (Language.LanguageType languageType in tempBackground.languages) {
                        languages.Add(languageType);
                        Debug.Log("Received language: " + languageType);
                    }
                    foreach (Feature feature in tempBackground.features) {
                        features.Add(feature);
                        Debug.Log("Received language: " + feature);
                    }

                } else {
                    Debug.LogError("Background not found: " + background);
                }
            } else {
                Debug.LogError("BackgroundDataSO is null");
            }
        }
    }

    private void ReceiveRaceData() {
        if ( raceData != null) {
            Race tempRace = new Race(
                raceData.races[(int)characterRace].raceType,
                raceData.races[(int)characterRace].size,
                raceData.races[(int)characterRace].speed,
                raceData.races[(int)characterRace].senses,
                raceData.races[(int)characterRace].abilityScoreIncrease,
                raceData.races[(int)characterRace].skillProficiencies,
                raceData.races[(int)characterRace].toolProficiencies,
                raceData.races[(int)characterRace].languages,
                raceData.races[(int)characterRace].weaponProficiencies,
                raceData.races[(int)characterRace].features);
            if (tempRace != null) {
                size = tempRace.size;
                speed = tempRace.speed; 
                
                foreach (Sense sense in tempRace.senses) {
                    Debug.Log("Received sense: " + sense);
                    senses.Add(sense);  
                }
                foreach (AbilityScoreIncrease abilityScoreIncrease in tempRace.abilityScoreIncrease) {
                    ChangeAbilityScore(abilityScoreIncrease.abilityScore, abilityScoreIncrease.abilityType);
                }
                foreach (Skill.SkillType skillType in tempRace.skillProficiencies) {
                    Debug.Log("Received skillProficiencies: " + skillType);
                    skills[(int)skillType].proficiency = true;
                }
                foreach (Tool.ToolProficiency skillType in tempRace.toolProficiencies) {
                    toolProficiencies.Add(skillType);
                    Debug.Log("Received toolProficiencies: " + skillType);
                }
                foreach (Language.LanguageType languageType in tempRace.languages) {
                    languages.Add(languageType);
                    Debug.Log("Received language: " + languageType);
                }
                foreach (Weapon.WeaponName weaponProficiency in tempRace.weaponProficiencies) {
                    weaponProficiencies.Add(weaponProficiency);
                    Debug.Log("Received weaponProficiency: " + weaponProficiency);
                }
                foreach (Feature feature in tempRace.features) {
                    features.Add(feature);
                    Debug.Log("Received feature: " + feature);
                }

            }
        }
    }

    private void ReceiveWeapons() {
        foreach (Weapon weapon in weaponInventoryData.weaponInventory) {
            Weapon newWeapon = new Weapon(
                weapon.weaponTitle,
                weapon.weaponName,
                weapon.weaponType,
                weapon.weaponProperty,
                weapon.weaponCost,
                weapon.weaponWeight,
                weapon.weaponAttacks);
            weapons.Add(newWeapon);
        }
    }

    private void ReceiveWeaponActions() {
        if (weapons.Count > 0) {
            foreach (Weapon weapon in weapons) {
                foreach (WeaponAttack weaponAttack in weapon.weaponAttacks) {
                    weaponActions.Add(new WeaponAction(
                        ActionType.MainAction,
                        weapon.weaponTitle + " " + weaponAttack.title,
                        weaponAttack.damageRoll,
                        weaponAttack.damageType,
                        /*weaponAttack.attackRollModifier,*/
                        weaponAttack.attackRange));
                }
            }
        }
    }


    // CONTEXT MENU METHODS

    [ContextMenu("Initialize Character")]
    private void InitializeCharacter() {
        InitializeLevel();
        InitializeAbilities();
        InitializeSkills();
        InitializeToolProficiencies();
        InitializeLanguages();
        InitializeFeatures();
        InitializeSenses();
        InitializeWeapons();
        InitializeWeaponActions();

        isInitialized = true;
    }

    [ContextMenu("Receive Inherited Properties")]
    private void ReceiveInheritedProperties() {
        ReceiveBackgroundData();
        ReceiveRaceData();


        receivedProperties = true;
    }

    [ContextMenu("Receive Inventory")]
    private void ReceiveInventory() {
        ReceiveWeapons();
    }
     
    [ContextMenu("Level Up")]
    private void LevelUp() {
        Level nextLevel = new Level(
            levelData.levels[level].level, 
            levelData.levels[level].experienceToNextLevel, 
            levelData.levels[level].totalExperienceToNextLevel,
            levelData.levels[level].proficiencyBonus);
        level = nextLevel.level;
        experienceToNextLevel = nextLevel.experienceToNextLevel;
        totalExperienceToNextLevel = nextLevel.totalExperienceToNextLevel;
        currentExperience = 0;
        proficiencyBonus = nextLevel.proficiencyBonus;
    }

    [ContextMenu("Generate Character Actions")]
    private void GenerateCharacterActions() {
        ReceiveWeaponActions();
    }

    // PUBLIC METHODS

    public int GetAbilityModifier(Ability.AbilityType abilityType) {
        return abilities[(int)abilityType].Modifier;
    }

    public int GetSkillModifier(Skill.SkillType skillType) {
        return skills[(int)skillType].SkillModifier;
    }

    public int GetArmorClass() {
        CalculateArmorClass();
        return armorClass;
    }

    public int GetInitiative() {
        CalculateInitiative();
        return initiative;
    }


    // OTHER METHODS

    private void ChangeAbilityScore(int amount, Ability.AbilityType abilityType) {
        Ability ability = abilities[(int)abilityType];
        ability.abilityScore += amount;
    }

    private void CalculateInitiative() {
        if (abilities.Count > 0 && abilities[(int)Ability.AbilityType.Dexterity] != null) {
            initiative = GetAbilityModifier(Ability.AbilityType.Dexterity);
            initiative = Mathf.Clamp(initiative, 0, 100);
        } else {
            initiative = -69; // Set a default value if the necessary objects are not available
        }
    }

    private void CalculateArmorClass() {
        if (abilities.Count > 0 && abilities[(int)Ability.AbilityType.Dexterity] != null) {
            armorClass = 10 + GetAbilityModifier(Ability.AbilityType.Dexterity);
            armorClass = Mathf.Clamp(armorClass, 0, 100);
        } else {
            armorClass = -69; // Set a default value if the necessary objects are not available
        }
    }

}

