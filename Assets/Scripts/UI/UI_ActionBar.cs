using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ActionBar : MonoBehaviour {
    private Transform mainActionsTransform;
    private Transform moveActionsTransform;
    private Transform healthBarTransform;
    private Transform moveBarTransform;

    private UI_MainActionBar mainActionBar;
    private UI_MoveActionBar moveActionBar;
    private UI_HealthBar healthBar;
    private UI_MoveBar moveBar;

    private void Awake() {
        FindActions();
        FindHealthBar();
        FindMoveBar();
    }

    private void Start() {
        PlayerManager.GetInstance().OnPlayerSelected += UI_ActionBar_OnPlayerSelected;
    }

    private void OnDestroy() {
        PlayerManager.GetInstance().OnPlayerSelected -= UI_ActionBar_OnPlayerSelected;
    }

    private void UI_ActionBar_OnPlayerSelected(Player player) {
        healthBar.SetPlayer(player);
        moveBar.SetPlayer(player);

        player.UpdateBars();
    }

    private void FindActions() {
        mainActionsTransform = transform.Find("UI_MainActions");
        if (mainActionsTransform != null) {
            mainActionBar = mainActionsTransform.GetComponent<UI_MainActionBar>();
        } else {
            Debug.LogWarning("UI_MainActions was not found");
        }


        moveActionsTransform = transform.Find("UI_MoveActions");
        if (moveActionsTransform != null) {
            moveActionBar = moveActionsTransform.GetComponent<UI_MoveActionBar>();
        } else {
            Debug.LogWarning("UI_MoveActions was not found");
        }
    }

    private void FindHealthBar() {
        healthBarTransform = transform.Find("UI_HealthBar");
        if (healthBarTransform != null) {
            healthBar = healthBarTransform.GetComponent<UI_HealthBar>();
            if (healthBar == null) {
                Debug.LogWarning("healthBar component was not found");
            }
        } else {
            Debug.LogWarning("UI_HealthBar was not found");
        }
    }

    private void FindMoveBar() {
        moveBarTransform = transform.Find("UI_MoveBar");
        if (moveBarTransform != null) {
            moveBar = moveBarTransform.GetComponent<UI_MoveBar>();
            if (moveBar == null) {
                Debug.LogWarning("moveBar component was not found");
            }
        } else {
            Debug.LogWarning("UI_MoveBar was not found");
        }
    }

}
