using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(CharacterDataSO))]
public class CharacterDataSOEditor : Editor {
    
    public override void OnInspectorGUI() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("characterName"));
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("characterRace"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("characterClass"));
        EditorGUI.indentLevel--;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("level"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentExperience"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("experienceToNextLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalExperienceToNextLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("characterAlignment"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("characterBackground"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("inspiration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("proficiencyBonus"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("initiative"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("armorClass"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hitPoints"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("temporaryHitPoints"));


        GUILayout.Space(10f);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilities"), true);

        GUILayout.Space(10f);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("skills"), true);

        GUILayout.Space(10f);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("toolProficiencies"), true);

        serializedObject.ApplyModifiedProperties();
    }




}
