using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private UI_MainActionBarActionSlot mainActionBarSlot;
    private UI_MoveActionBarActionSlot moveActionBarSlot;


    private void Awake() {
        if (TryGetComponent(out UI_MainActionBarActionSlot mainActionBarSlot)) {
            this.mainActionBarSlot = mainActionBarSlot;
        }

        if (TryGetComponent(out UI_MoveActionBarActionSlot moveActionBarSlot)) {
            this.moveActionBarSlot = moveActionBarSlot;
        }
    }

    public string GetTooltipText() {
        if (mainActionBarSlot != null) {
            return mainActionBarSlot.GetDescription();
        } else if (moveActionBarSlot != null) {
            return moveActionBarSlot.GetDescription();
        } else {
            return "Action is null";
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //Debug.Log("POINTER ENTER");
        System.Func<string> getTooltipTextFunc = () => {
            return GetTooltipText();
        };
        UI_Tooltip.ShowTooltip_Static(getTooltipTextFunc);
    }

    public void OnPointerExit(PointerEventData eventData) {
        //Debug.Log("POINTER EXIT");

        UI_Tooltip.HideTooltip_Static();
    }
}

