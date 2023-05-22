using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;


public class UI_PortraitBar : MonoBehaviour {
    private Transform playerPortraitTemplate;
    private List<Player> players;





    private void Awake() {
        players = new List<Player>();
        playerPortraitTemplate = transform.Find("PlayerPortraitTemplate");
        playerPortraitTemplate.gameObject.SetActive(false);

    }

    private void Start() {
        EntityHandler.GetInstance().OnPlayerEntityAdded += UI_PortraitBar_OnPlayerEntityAdded;
    }

    private void OnDestroy() {
        EntityHandler.GetInstance().OnPlayerEntityAdded -= UI_PortraitBar_OnPlayerEntityAdded;
    }

    private void UI_PortraitBar_OnPlayerEntityAdded(Player player) {
        players.Add(player);
        //player.UpdateBars();
        UpdateVisual();
        
    }


    public void UpdateVisual() {
        float startPosition = 0f;

        foreach (Transform child in transform) {
            // Clear the bar
            if (child == playerPortraitTemplate) {
                continue;
            }

            Destroy(child.gameObject);

        }

        for (int i = 0; i < players.Count; i++) {
            Player player = players[i];
            Transform playerPortraitTransform = Instantiate(playerPortraitTemplate, transform);
            playerPortraitTransform.gameObject.SetActive(true);

            UI_PlayerPortrait playerPortrait = playerPortraitTransform.GetComponent<UI_PlayerPortrait>();
            playerPortrait.Initialize();
            playerPortrait.SetPlayer(player);

            RectTransform playerPortraitRectTransform = playerPortraitTransform.GetComponent<RectTransform>();
            //playerPortraitRectTransform.anchoredPosition = new Vector2(0f, startPosition + (-150f * i));
            playerPortraitRectTransform.anchoredPosition = new Vector2( startPosition + (+105f * i), 0f);
            playerPortrait.Show();

        }

    } 


}
