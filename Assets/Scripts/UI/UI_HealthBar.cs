using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealthBar : MonoBehaviour {
    private Transform bar;

    private Player player;
    private bool isSubscribed = false;


    public void SetPlayer(Player player) {
        bar = transform.Find("Bar");

        if (isSubscribed && this.player != null) {
            this.player.OnHealthPercentageChanged -= Player_OnHealthPercentageChanged;
        }

        this.player = player;

        if (this.player != null) {
            this.player.OnHealthPercentageChanged += Player_OnHealthPercentageChanged;
            isSubscribed = true;
        }
    }


    private void Player_OnHealthPercentageChanged(float healthPercentage) {
        if (bar != null) {
            bar.localScale = new Vector3(healthPercentage, 1, 1);
        }
    }
}
 