using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameObject pauseCinemachine;

    
     

    private bool isGamePaused = false;
    
    void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;



    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }



    private void TogglePauseGame() {
        isGamePaused = !isGamePaused;
        if (isGamePaused) {
            //Time.timeScale = 0f;
            pauseCinemachine.SetActive(true);
            pauseMenu.SetPage12(true);
            pauseMenu.SetPage21(false);
        } else {
            pauseCinemachine.SetActive(false);
            pauseMenu.SetPage12(false);
            pauseMenu.SetPage21(true);
            //Time.timeScale = 1f;
        }
    }


}
