using DnD5eData;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IAttackable , IAttacker, IHaveDescription{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private bool manualInput = false;

    [SerializeField] private Character character;

    [SerializeField] private float playerHeight = 1.7f;
    [SerializeField] private float playerRadius = .4f;

    [SerializeField] private int gameMoveRange;
    [SerializeField] private int remainingGameMoveRange;

    [SerializeField] private int combatInitiative;

    public event Action<int> OnInitiativeRolled;
    public event Action<float> OnHealthPercentageChanged;
    public event Action<float> OnMovePercentageChanged;
    
    private int gridMoveRange;

    private bool isWalking;
    private bool isDead;

    

 
    // ---------------- Mono Methods ----------------

    private void Start() {

        if (character == null) {
            if (TryGetComponent(out Character character)) {
                this.character = character;
            } else {
                Debug.LogWarning("Character was not found on Game Object.");
            }
        }
        if (character != null) {
            if (character.initializationComplete) {
                gameMoveRange = character.speed;
                remainingGameMoveRange = gameMoveRange;
            }

        }

    }

    private void Update() {
        if (manualInput) {
            GetComponent<HandleMovementKeys>().HandleMovement();
        } else {
            GetComponent<HandleMovementPathfinding>().HandleMovement();
        }
    }


    // ---------------- Public Methods ----------------

    // Get Methods
    public Inventory GetInventory() {
        return character.GetInventory();
    }

    public string GetDescription() {
        return character.name + "\n" +
               character.characterRace + " " +
               character.characterClass + "\n" +
               "LvL:  " + character.level;
    }

    public Sprite GetSprite() {
        return character.GetSprite();
    }

    public Vector3 GetWorldPosition() {
        return transform.position;
    }

    public Quaternion GetRotation() {
        return transform.rotation;
    }

    public float GetHeight() {
        return playerHeight;
    }

    public float GetRadius() {
        return playerRadius;
    }

    public int GetGridMoveRange() {
        gridMoveRange = GetGameMoveRange() / 5;
        return gridMoveRange;
    }

    public int GetGameMoveRange() {
        return gameMoveRange;
    }

    public int GetRemainingGameMoveRange() {
        return remainingGameMoveRange;
    }

    public void SetRemainingGameMoveRange(int remainingGameMoveRange) {
        this.remainingGameMoveRange = remainingGameMoveRange;
        OnMovePercentageChanged?.Invoke(GetRemainingGameMoveRange() / (float)GetGameMoveRange());
    }

    public int GetArmorClass() {
        return character.GetArmorClass();
    }

    public int GetInitiativeModifier() {
        if (character != null) {
            return character.GetInitiative();
        } else {
            Debug.Log("Character is null");
            return 0;
        }
    }

    public int GetCombatInitiative() {
        return combatInitiative;
    }

    public int GetCurrentHP() {
        return character.GetCurrentHP();
    }

    public int GetMaxHP() {
        return character.GetMaxHP();
    }

    // Combat Methods

    public int RollInitiative() {
        combatInitiative = GetInitiativeModifier() + DiceSystem.GetInstance().Roll(DiceRoll.DiceType.d20);
        OnInitiativeRolled?.Invoke(combatInitiative);
        return combatInitiative;

    }

    public void Attack(IAttackable target, Action onAttackComplete) {
        MoveToAttack(target.GetWorldPosition(), () => {
            PathNode playerNode = GridCombatSystem.GetInstance().GetGrid().GetGridObject(this.GetWorldPosition(),out bool playerNodeExists);
            PathNode targetNode = GridCombatSystem.GetInstance().GetGrid().GetGridObject(target.GetWorldPosition(),out bool targetNodeExists);
            Vector3 attackDir = GridCombatSystem.GetInstance().CalculateDirectionVector(playerNode, targetNode);
            PlayAnimAttack(attackDir, null, () => {
                bool attackHit = character.MakeAttackRoll(target.GetArmorClass());
                if (attackHit) {
                    int damageRolled = character.MakeDamageRoll();
                    target.ReceiveAttack(attackHit, damageRolled);
                }
                else {
                    target.ReceiveAttack(attackHit, 0);
                }
                onAttackComplete();
            });
        });
    }

    public void ReceiveAttack(bool attackHit, int damage) {
        if (attackHit) {
            character.ReceiveDamage(damage);
            OnHealthPercentageChanged?.Invoke(character.GetCurrentHP() /(float)character.GetMaxHP());
        }
    }

    public void MoveTo(Vector3 targetPosition, Action onReachedTargetPosition) {
        if(!manualInput) {
            GetComponent<HandleMovementPathfinding>().SetTargetPosition(targetPosition, onReachedTargetPosition);
        }
    }

    public void MoveToAttack(Vector3 targetPosition, Action onReachedTargetPosition) {
        if(!manualInput) {
            GetComponent<HandleMovementPathfinding>().SetTargetPositionForAttack(targetPosition, onReachedTargetPosition);
        }
    }

    // Animator Methods

    public bool IsDead() {
         isDead = character.IsDead();
         return isDead;
    }

    public bool IsWalking() {
        return manualInput ? GetComponent<HandleMovementKeys>().IsWalking() : GetComponent<HandleMovementPathfinding>().IsWalking();
    }



    public void PlayAnimAttack(Vector3 attackDir, Action onHit, Action onComplete) {
        Quaternion targetRotation = Quaternion.LookRotation(attackDir, Vector3.up);
        StartCoroutine(RotatePlayer(targetRotation, () => {
            GetComponentInChildren<PlayerAnimator>().PlayAttackAnimation(onComplete);
        }));
    }

    private IEnumerator RotatePlayer(Quaternion targetRotation, Action onComplete) {
        float duration = 0.5f; // duration of the rotation
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        onComplete?.Invoke();
    }


    //Other Methods
    public void UpdateBars() {
        OnHealthPercentageChanged?.Invoke(GetCurrentHP() / (float)GetMaxHP());
        OnMovePercentageChanged?.Invoke(GetRemainingGameMoveRange() / (float)GetGameMoveRange());
    }


}
