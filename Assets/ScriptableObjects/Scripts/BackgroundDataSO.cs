using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewBackgroundData", menuName = "ScriptableObjects/Background Data")]
public class BackgroundDataSO : ScriptableObject {
    public List<Background> backgrounds;

    private void Reset() {
        backgrounds = new List<Background>();
    }
}
