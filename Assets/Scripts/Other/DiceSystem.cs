using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DiceRoll;

public class DiceSystem : MonoBehaviour {
    private static DiceSystem Instance;


    private void Awake() {
        Instance = this;
    }

    void Start() { }

    public static DiceSystem GetInstance() {
        return Instance;
    }

    public int Roll(DiceType diceType) {
        return Random.Range(1, GetSidesCount(diceType) + 1);
    }


    public int GetSidesCount(DiceType diceType) {
        // Get the enum value as a string
        string diceTypeString = diceType.ToString();
        // Remove the 'd' prefix from the string
        string numberString = diceTypeString.Substring(1);
        // Parse the number string to an integer
        int diceNumber = int.Parse(numberString);
        return diceNumber;
    }
}
