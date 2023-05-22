using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZone : MonoBehaviour {

    Dictionary<Dice, Vector3> diceVelocities = new Dictionary<Dice, Vector3>();

    
    Vector3 diceVelocity;


    private void Update() {
        Dice[] diceObjects = FindObjectsOfType<Dice>();
        foreach (Dice dice in diceObjects) {
            diceVelocities[dice] = dice.diceVelocity;
        }
        //todo: need to bring dice instances in here to check for dicevelocity.
        //diceVelocity = Dice.diceVelocity;
    }
     
   private void OnTriggerStay(Collider other) {
        if (diceVelocity.x == 0f &&
            diceVelocity.y == 0f &&
            diceVelocity.z == 0f) {
            switch (other.gameObject.name) {
                case "Side6":
                    Debug.Log(other.gameObject.name);
                    break;
                case "Side5":
                    Debug.Log(other.gameObject.name);
                    break;
                case "Side4":
                    Debug.Log(other.gameObject.name);
                    break;
                case "Side3":
                    Debug.Log(other.gameObject.name);
                    break;
                case "Side2":
                    Debug.Log(other.gameObject.name);
                    break;
                case "Side1":
                    Debug.Log(other.gameObject.name);
                    break;
            }
        }
    }

}
