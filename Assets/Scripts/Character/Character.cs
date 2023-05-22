using DnD5eData;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour { 
    /** READ ONLY! */
    [SerializeField] private CharacterDataSO baseCharacterData;
    /**------------*/
    private Player player;
    [HideInInspector]
    public CharacterDataSO characterData;
    public bool initializationComplete = false;

    [Header("ADD PLAYER PORTRAIT HERE")]
    [SerializeField] public Sprite characterIcon;

    [Header("DO NOT EDIT")]
    [SerializeField] public string characterName;
    [SerializeField] public string characterRace;
    [SerializeField] public string characterClass;
    [SerializeField] public int level;
    [SerializeField] public int proficiencyBonus;
    [SerializeField] public int AC;
    private int HP;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    
    [SerializeField] private bool isDead;
    [SerializeField] public int speed;
    [SerializeField] public int rolledInitiative;
    [SerializeField] public List<Ability> abilities;


    [SerializeField] public Weapon mainHandWeapon;


    private Inventory inventory;



    private void Awake() {
        Debug.Log("awake init:" + initializationComplete);
        if (initializationComplete) {
            return;
        }

        player = GetComponent<Player>();
        player.OnInitiativeRolled += Player_OnInitiativeRolled;

        if (baseCharacterData != null) {
            if (baseCharacterData.isInitialized && baseCharacterData.receivedProperties) {
                // Make a copy of the base character data
                characterData = ScriptableObject.CreateInstance<CharacterDataSO>();
                characterData.Init(baseCharacterData);

                characterName = characterData.characterFullName;
                characterRace = characterData.characterRace.ToString();
                characterClass = characterData.characterClass.ToString();
                level = characterData.level;
                proficiencyBonus = characterData.proficiencyBonus;
                AC = characterData.GetArmorClass();
                maxHP = characterData.hitPoints;
                currentHP = characterData.hitPoints;
                speed = characterData.speed;
                abilities = new List<Ability>();
                foreach (Ability ability in characterData.abilities) {
                    abilities.Add(ability);
                }
                 
                //---temp
                mainHandWeapon = new Weapon(
                    characterData.weapons[0].weaponTitle,
                    characterData.weapons[0].weaponName,
                    characterData.weapons[0].weaponType,
                    characterData.weapons[0].weaponProperty,
                    characterData.weapons[0].weaponCost,
                    characterData.weapons[0].weaponWeight,
                    characterData.weapons[0].weaponAttacks
                );
                //-------

                rolledInitiative = 0;
                isDead = false;

                inventory = new Inventory();

                initializationComplete = true;
                gameObject.name = characterName;
            }
        }
    }

    private void Start() {

    }

    private void OnDestroy() {
        player.OnInitiativeRolled -= Player_OnInitiativeRolled;
    }

    private void Player_OnInitiativeRolled(int rolledInitiative) {
        this.rolledInitiative = rolledInitiative;
    }




    // PUBLIC METHODS

    public bool IsDead() {
        return isDead;
    }

    public Inventory GetInventory() {
        return inventory;
    }

    public Sprite GetSprite() {
        return characterIcon;
    }

     
    public int GetAbilityModifier(Ability ability) {
        return abilities[(int)ability.type].Modifier;
    }

    public int GetInitiative() {
        if (characterData != null) {
            return characterData.initiative;
        } else {
            Debug.Log("characterData is null");
            return 0;
        }
    }

    public int GetArmorClass() {
        if (characterData != null) {
            return characterData.armorClass;
        } else {
            Debug.Log("characterData is null");
            return 0;
        }
    }

    public int GetCurrentHP() {
        return currentHP;
    }

    public int GetMaxHP() {
        return maxHP;
    }

    public bool MakeAttackRoll(int targetArmorClass) {
        int attackRoll = DiceSystem.GetInstance().Roll(DiceRoll.DiceType.d20);
        int abilityModifier;
        if (mainHandWeapon.weaponProperty == Weapon.WeaponProperty.Finesse) {
            abilityModifier = GetAbilityModifier((abilities[(int)Ability.AbilityType.Dexterity]));
        }
        else {
            abilityModifier = GetAbilityModifier((abilities[(int)Ability.AbilityType.Strength]));
        }
        
        attackRoll += abilityModifier;

        if (characterData.weaponProficiencies.Contains(mainHandWeapon.weaponName)) {
            attackRoll += proficiencyBonus;
        }

        Debug.Log(name + "ATK: " + attackRoll + " d20" );
        if (attackRoll >= targetArmorClass) {
            Debug.Log( "HIT! target AC:" + targetArmorClass);
            return true;
        }
        else {
            Debug.Log("MISS! target AC:" + targetArmorClass);
            return false;
        }
    }

    public int MakeDamageRoll() {
        int damageRoll = DiceSystem.GetInstance().Roll(mainHandWeapon.weaponAttacks[0].damageRoll);
        return damageRoll;
    }

    public void ReceiveDamage(int damage) {
        currentHP -= damage;
        if (currentHP <= 0) {
            isDead = true;
        }
    }
}
