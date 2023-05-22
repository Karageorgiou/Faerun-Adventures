using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UI_PlayerBar : MonoBehaviour {
    private Transform playerSlotTemplate;
    private Transform background;
    private Transform border;
    private List<PlayerSlot> playerSlots;





    private void Awake() {
        playerSlots = new List<PlayerSlot>();
        playerSlotTemplate = transform.Find("PlayerSlotTemplate");
        playerSlotTemplate.gameObject.SetActive(false);
        background = transform.Find("Background");
        border = transform.Find("Border");

    }

    private void Start() {
        EntityHandler.GetInstance().OnPlayerEntityAdded += UI_PlayerBar_OnPlayerEntityAdded;
    }

    private void UI_PlayerBar_OnPlayerEntityAdded(Player player) {
        PlayerSlot playerSlot = new PlayerSlot(player, () => PlayerManager.GetInstance().SelectPlayer(player));
        //if (!playerSlots.Contains(playerSlot)) {
            playerSlots.Add(playerSlot);
            UpdateVisual();
        //}
    }


    public void UpdateVisual() {
        float startPosition = -147.5f;

        foreach (Transform child in transform) { // Clear the bar
            if (child == playerSlotTemplate ||
                child == background ||
                child == border) {
                continue;
            }
            Destroy(child.gameObject);

        }

        for (int i = 0; i < playerSlots.Count; i++) {
            PlayerSlot playerSlot = playerSlots[i];
            Transform playerSlotTransform = Instantiate(playerSlotTemplate, transform);
            playerSlotTransform.gameObject.SetActive(true);    
            RectTransform playerSlotRectTransform = playerSlotTransform.GetComponent<RectTransform>();
            playerSlotRectTransform.anchoredPosition = new Vector2(startPosition + (125f * i), 0f);
            playerSlotTransform.Find("Icon").GetComponent<Image>().sprite = playerSlot.GetSprite();

            playerSlotTransform.GetComponent<UI_PlayerBarPlayerSlot>().SetPlayerSlot(playerSlot);
        }

    }






    public class PlayerSlot {
        public Player player;
        public Action selectPlayerAction;

        public PlayerSlot(Player player, Action selectPlayerAction) {
            this.player = player;
            this.selectPlayerAction = selectPlayerAction;
        }

        public Sprite GetSprite() {
            return player.GetSprite();
        }
    }




}
