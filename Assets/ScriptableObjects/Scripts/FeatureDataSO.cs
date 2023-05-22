using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewFeatureData", menuName = "ScriptableObjects/Feature Data")]
public class FeatureDataSO : ScriptableObject {
    public List<Feature> features;

    private void Reset() {
        features = new List<Feature>();
    }
}
