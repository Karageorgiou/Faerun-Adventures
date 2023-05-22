using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewSkillData", menuName = "ScriptableObjects/Skill Data")]
public class SkillDataSO : ScriptableObject {
    public List<Skill> skills;

    private void Reset() {
        skills = new List<Skill>();
    }
}
