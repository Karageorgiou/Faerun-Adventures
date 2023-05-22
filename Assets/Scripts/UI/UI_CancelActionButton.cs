using GameUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CancelActionButton : MonoBehaviour, IPointerDownHandler {
    public void OnPointerDown(PointerEventData eventData) {
        if (GridCombatSystem.GetInstance().GetState() == GridCombatSystem.State.Busy) {
            return;
        }
        GridCombatSystem.GetInstance().CancelAction();
    }




}
