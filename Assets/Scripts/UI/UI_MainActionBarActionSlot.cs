using GameUtils;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MainActionBarActionSlot : MonoBehaviour, IPointerDownHandler {
    private Transform iconPressed;
    //public bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData) {
        if (GridCombatSystem.GetInstance().GetState() == GridCombatSystem.State.Busy) {
            return;
        }

        GridCombatSystem.GetInstance().CancelAction();
        GridCombatSystem.GetInstance().SetTurnState(ActionType.MainAction);
        //isPressed = true;
    }

    private void Awake() {
        iconPressed = transform.Find("IconPressed");
        iconPressed.gameObject.SetActive(false);
    }

    private void Start() {
        GridCombatSystem.GetInstance().OnMainActionTriggered += UI_MainActionBarActionSlot_OnMainActionTriggered;
        GridCombatSystem.GetInstance().OnCancelActionTriggered += UI_MainActionBarActionSlot_OnCancelActionTriggered;
    }

    private void OnDestroy() {
        GridCombatSystem.GetInstance().OnMainActionTriggered -= UI_MainActionBarActionSlot_OnMainActionTriggered;
        GridCombatSystem.GetInstance().OnCancelActionTriggered -= UI_MainActionBarActionSlot_OnCancelActionTriggered;
    }

    private void UI_MainActionBarActionSlot_OnCancelActionTriggered(Player obj) {
        if (iconPressed != null) {
            iconPressed.gameObject.SetActive(false);
        }
    }

    private void UI_MainActionBarActionSlot_OnMainActionTriggered(Player obj) {
        if (iconPressed != null) {
            iconPressed.gameObject.SetActive(true);
        }
    }

    public string GetDescription() {
        if (PlayerManager.GetInstance().GetSelectedPlayer() != null) {
            return PlayerManager.GetInstance().GetSelectedPlayer().GetDescription();
        }
        else {
            return "There is no player selected";
        }
    }


}
