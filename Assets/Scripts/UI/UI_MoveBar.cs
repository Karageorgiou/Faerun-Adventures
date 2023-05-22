using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MoveBar : MonoBehaviour {
    private Transform bar;

    private Player player;
    private bool isSubscribed = false;


    public void SetPlayer(Player player) {
        bar = transform.Find("Bar");

        if (isSubscribed && this.player != null) {
            this.player.OnMovePercentageChanged -= Player_OnMovePercentageChanged;
        }

        this.player = player;

        if (this.player != null) {
            this.player.OnMovePercentageChanged += Player_OnMovePercentageChanged;
            isSubscribed = true;
        }
    }

    private void Player_OnMovePercentageChanged(float movePercentage) {
        if (bar != null) {
            bar.localScale = new Vector3(movePercentage, 1, 1);
        }
    }
}
