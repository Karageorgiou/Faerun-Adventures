using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class CombatTracker : MonoBehaviour {
    private static CombatTracker Instance;

    public event Action<int> OnTurnStarted;
    public event Action<int> OnRoundStarted;
    public event Action<List<Player>> OnCombatStarted;

    private Dictionary<Player, int> playerInitiatives = new Dictionary<Player, int>();
    private int currentTurnIndex = 0;
    private int currentRound = 0;
    private bool combatStarted = false;


    private void Awake() {
        Instance = this;
    }

    public static CombatTracker GetInstance() {
        return Instance;
    }



    public void AddPlayerToCombat(Player player) {
        if (playerInitiatives.ContainsKey(player)) {
            Debug.Log("Player is already inside the combat tracker!");
        } else {
            playerInitiatives.Add(player, 0);
            Debug.Log("Added player to Combat Tracker.");
        }
    }
    
    public void AddPlayerToCombat(Player player, int initiative) {
        if (playerInitiatives.ContainsKey(player)) {
            Debug.Log("Player is already inside the combat tracker!");
        } else {
            playerInitiatives.Add(player, initiative);
            Debug.Log("Added player to Combat Tracker.");
        }
    }

    public void RemovePlayerFromCombat(Player player) {
        if (!playerInitiatives.ContainsKey(player)) {
            Debug.Log("Player does not exist in the combat tracker!");
        } else {
            Debug.Log("Removed player from Combat Tracker.");
            playerInitiatives.Remove(player);
        }
    }

    public bool StartCombat() {
        if (playerInitiatives.Count < 2) {
            Debug.Log("Can't start combat with one unit.");
            return false;
        } else {
            Debug.Log("Combat started");
            RollInitiatives();
            SortPlayersByInitiative();
            combatStarted = true;
            OnCombatStarted?.Invoke(GetPlayerList());
            StartRound(0);
            return true;
        }
        
    }

    public void EndCombat() {
        Debug.Log("Combat Ended");
        playerInitiatives.Clear();
        currentTurnIndex = 0;
        currentRound = 0;
        combatStarted = false;
    }

    public bool CombatStarted() {
        return combatStarted;
    }

    private void RollInitiatives() {
        foreach (Player player in playerInitiatives.Keys.ToList()) {
            player.RollInitiative();
            int initiative = player.GetCombatInitiative();
            playerInitiatives[player] = initiative;
        }
    }

    


    private void StartRound(int roundIndex) {
        currentRound = roundIndex;
        OnRoundStarted?.Invoke(currentRound);
        StartTurn(0);
    }

    private void EndRound() {
        currentTurnIndex = 0;
        currentRound++;
        StartRound(currentRound);
    }



    private void StartTurn(int turnIndex) {
        currentTurnIndex = turnIndex;
        OnTurnStarted?.Invoke(currentTurnIndex);
    }

    public void EndTurn() {
        currentTurnIndex++;
        if (currentTurnIndex >= playerInitiatives.Count) {
            EndRound();
        } else {
            StartTurn(currentTurnIndex);
        }
    }



    private void SortPlayersByInitiative() {
        var sortedInitiatives = playerInitiatives.OrderByDescending(pair => pair.Value);
        playerInitiatives = sortedInitiatives.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public Player GetCurrentPlayer() {
        return playerInitiatives.Keys.ToList()[currentTurnIndex];
    }

    private List<Player> GetPlayerList() {
        return new List<Player>(playerInitiatives.Keys);
    }
}
