using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleMovementKeys : MonoBehaviour, IHandleMovement {
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;


    [SerializeField] private GameInput gameInput;
    private Player player;

    private bool isWalking;
    int layerMask;


    private Door interactableDoor;
    private Chest interactableChest;



    // ---------------- Event Subscribers ----------------

    private void GameInput_OnInteraction(object sender, EventArgs e) {
        if (interactableDoor != null) {
            interactableDoor.Interact(transform);
        }
        if (interactableChest != null) {
            interactableChest.Interact(transform);
        }
    }



    private void Start() {
        if (gameInput == null) {
            gameInput = FindObjectOfType<GameInput>();
        }
        if (gameInput == null) {
            Debug.LogError("GameInput component not found in the scene.");
        } else {
            gameInput.OnInteraction += GameInput_OnInteraction;
        }

        player = GetComponent<Player>();
        layerMask = ~(1 << LayerMask.NameToLayer("MoveThrough"));
    }

    public void HandleMovement() {
        Vector2 inputVector = gameInput.GetInputVectorNormalized();
        Vector3 movementDirection = new Vector3(inputVector.x, 0, inputVector.y);
        float movementDistance = movementSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player.GetHeight(), player.GetRadius(), movementDirection, movementDistance, layerMask);
        if (!canMove) {
            Vector3 movementDirectionX = new Vector3(movementDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player.GetHeight(), player.GetRadius(), movementDirectionX, movementDistance, layerMask);
            if (canMove) {
                // Can ONLY move on X-AXIS
                movementDirection = movementDirectionX;
            } else {
                Vector3 movementDirectionZ = new Vector3(0, 0, movementDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player.GetHeight(), player.GetRadius(), movementDirectionZ, movementDistance, layerMask);
                if (canMove) {
                    // Can ONLY move on Z-AXIS
                    movementDirection = movementDirectionZ;
                } else {
                    // Can't move in any direction.
                }
            }
        }
        
        if (canMove) {
            transform.position += movementDirection * movementDistance;
            transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotationSpeed);
        }
        isWalking = movementDirection != Vector3.zero;
    }

    public bool IsWalking() {
        return isWalking;
    }

    public void SetTargetPosition(Vector3 targetPosition, Action onTargetPositionReached) { }


    private Vector3 lastInteractDirection;
    // ---------------- Private Methods ----------------
    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetInputVectorNormalized();
        Vector3 interactDirection = new Vector3(inputVector.x, 0, inputVector.y);
        float interactionDistance = 2f;

        if (interactDirection != Vector3.zero) {
            lastInteractDirection = interactDirection;
        }

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactionDistance)) {
            Debug.Log(raycastHit.collider);
            if (raycastHit.transform.TryGetComponent(out Chest chest)) {
                if (chest != interactableChest) {
                    interactableChest = chest;
                }
            } else {

                interactableChest = null;
            }
            if (raycastHit.transform.TryGetComponent(out Door door)) {
                if (door != interactableDoor) {
                    interactableDoor = door;
                }
            } else {

                interactableDoor = null;
            }


        } else {
            interactableChest = null;
            interactableDoor = null;
            //Debug.Log("-");
        }
    }





}
