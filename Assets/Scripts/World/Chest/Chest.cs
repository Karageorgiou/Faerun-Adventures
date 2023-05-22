using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable, IHaveDescription {

    private string title;
    private bool isOpen = false;
    private bool isLocked = false;

    private void Start() {
        title = "Wooden Chest";
    }

    public string GetInteractionText() {
        return "Interacting with Chest";
    }

    public void Interact(Transform interactorTransform) {
        ToggleChest();
    }

    private void ToggleChest() {
        isOpen = !isOpen;
        Debug.Log("Interacting with chest");
    }

    public bool IsOpen() {
        return isOpen;
    }

    public string GetDescription() {
        return title + "\n" +
               "Locked: " + isLocked;
    }
}
