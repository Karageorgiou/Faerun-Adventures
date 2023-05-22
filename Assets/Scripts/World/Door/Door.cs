using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    private bool isOpen;

    public string GetInteractionText() {
        return "Interacting with Door";
    }

    public void Interact(Transform interactorTransform) {
        ToggleDoor();
    }

    private void ToggleDoor() {
        isOpen = !isOpen;
        Debug.Log("Opening Door");
    }

    public bool IsOpen() {
        return isOpen;
    }
}
