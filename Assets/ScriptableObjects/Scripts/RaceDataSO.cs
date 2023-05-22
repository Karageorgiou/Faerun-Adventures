using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewrRaceData", menuName = "ScriptableObjects/Race Data")]
public class RaceDataSO : ScriptableObject {
    public List<Race> races;

    private void Reset() {
        races = new List<Race>();
    }
}
