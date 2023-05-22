using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PlayerBarPlayerSlot : MonoBehaviour, IPointerDownHandler {

    UI_PlayerBar.PlayerSlot playerSlot;


    public void SetPlayerSlot(UI_PlayerBar.PlayerSlot playerSlot) {
        this.playerSlot = playerSlot;
    }


    public void OnPointerDown(PointerEventData eventData) {
        playerSlot.selectPlayerAction();
    }
}
 