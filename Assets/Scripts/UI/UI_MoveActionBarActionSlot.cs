using GameUtils;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MoveActionBarActionSlot : MonoBehaviour, IPointerDownHandler {
    private Transform iconPressed;
    //public bool isPressed;

    public void OnPointerDown(PointerEventData eventData) {
        if (GridCombatSystem.GetInstance().GetState() == GridCombatSystem.State.Busy) {
            return;
        }

        GridCombatSystem.GetInstance().CancelAction();
        GridCombatSystem.GetInstance().SetTurnState(ActionType.MoveAction);
        //isPressed = true;
    }



    private void Awake() {
        iconPressed = transform.Find("IconPressed");
        iconPressed.gameObject.SetActive(false);
    }

    private void Start() {
        GridCombatSystem.GetInstance().OnMoveActionTriggered += UI_MainActionBarActionSlot_OnMoveActionTriggered;
        GridCombatSystem.GetInstance().OnCancelActionTriggered += UI_MainActionBarActionSlot_OnCancelActionTriggered;
    }

    private void OnDestroy() {
        GridCombatSystem.GetInstance().OnMainActionTriggered -= UI_MainActionBarActionSlot_OnMoveActionTriggered;
        GridCombatSystem.GetInstance().OnCancelActionTriggered -= UI_MainActionBarActionSlot_OnCancelActionTriggered;
    }

    private void UI_MainActionBarActionSlot_OnCancelActionTriggered(Player obj) {
        if (iconPressed != null) {
            iconPressed.gameObject.SetActive(false);
        }
    }

    private void UI_MainActionBarActionSlot_OnMoveActionTriggered(Player obj) {
        if (iconPressed != null) {
            iconPressed.gameObject.SetActive(true);
        }
    }

    public string GetDescription() {
        if (PlayerManager.GetInstance().GetSelectedPlayer() != null) {
            return PlayerManager.GetInstance().GetSelectedPlayer().GetDescription() + "move";
        } else {
            return "There is no player selected";
        }
    }



    /*private void Update() {
        switch (isPressed) {
            case true when !iconPressed.gameObject.activeSelf:
                iconPressed.gameObject.SetActive(true);
                break;
            case false when iconPressed.gameObject.activeSelf:
                iconPressed.gameObject.SetActive(false);
                break;
        }
    }*/

}
