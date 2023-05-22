using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    private static PlayerManager Instance;

    [SerializeField] private LayerMask selectedPlayerLayerMask;
    [SerializeField] private LayerMask notSelectedPlayerLayerMask;

    [SerializeField] private List<Player> players = new List<Player>();
    [SerializeField] private Player selectedPlayer;
    private int selectedIndex = 0;


    public event Action<Player> OnPlayerSelected;

    public static PlayerManager GetInstance() {
        return Instance;
    }

    public Player GetSelectedPlayer() {
        if (players.Count == 0) {
            return null;
        }

        return selectedPlayer;
    }
     
    private void Awake() {
        Instance = this;

        
    }
    private void Start() {
        // Subscribe to the OnEntityAdded event
        EntityHandler.GetInstance().OnEntitySpawned += OnEntitySpawned;
        EntityHandler.GetInstance().OnPlayerEntityAdded += PlayerManager_OnPlayerEntityAdded;
    }

    

    private void OnDestroy() {
        // Unsubscribe from the OnEntityAdded event
        EntityHandler.GetInstance().OnEntitySpawned -= OnEntitySpawned;
    }

    private void Update() {
        /*if (!CombatTracker.GetInstance().CombatStarted()) {
            if (players.Count > 1) {
                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    // Cycle to the next player
                    selectedIndex = (selectedIndex + 1) % players.Count;
                    SelectPlayer(players[selectedIndex]);
                } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    // Cycle to the previous player
                    selectedIndex = (selectedIndex - 1 + players.Count) % players.Count;
                    SelectPlayer(players[selectedIndex]);
                }
            } else if (players.Count == 1) {
                if (selectedPlayer == null) {
                    SelectPlayer(players[0]);
                }
            }
        }*/
    }



    private void PlayerManager_OnPlayerEntityAdded(Player player) {
        players.Add(player);
    }



    private void OnEntitySpawned(Transform characterTransform) {
        // Check if the added entity has a Player component
        Player playerComponent = characterTransform.GetComponent<Player>();
        if (playerComponent != null && playerComponent.isActiveAndEnabled && !players.Contains(playerComponent)) {
            players.Add(playerComponent);
            //SelectPlayer(playerComponent);
        }
    }

    public void SelectPlayer(Player player) {
        if (players.Count > 0) {
            foreach (Player otherPlayer in players) {
                GameObject otherPlayerVisual = otherPlayer.transform.GetChild(0).gameObject;
                SwapLayer(otherPlayerVisual, "NoOutline", true);
            }
        }
        if (players.Contains(player)) {
            selectedPlayer = player;
            selectedIndex = players.IndexOf(player);
            GameObject playerVisual = selectedPlayer.transform.GetChild(0).gameObject;
            SwapLayer(playerVisual, "Outline", true);
            OnPlayerSelected?.Invoke(selectedPlayer);
        }
    }
        
    public void SwapLayer(GameObject obj, string layerName, bool swapChildren) {
        obj.layer = 0;
        obj.layer = LayerMask.NameToLayer(layerName);
        if (swapChildren) {
            foreach (Transform child in obj.GetComponentsInChildren<Transform>(true)) {
                child.gameObject.layer = 0;
                child.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
    }


} 
