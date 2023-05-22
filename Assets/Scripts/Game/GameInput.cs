using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameInput : MonoBehaviour {

    public static GameInput Instance { get; private set; }



    public EventHandler OnPauseAction;
    public EventHandler OnInteraction;


    private PlayerInputActions playerInputActions;





    private void Awake() {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Interact.performed += Interaction_performed;
    }

    private void Pause_performed(InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

   
    public void Interaction_performed(InputAction.CallbackContext context) {
            OnInteraction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetInputVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
