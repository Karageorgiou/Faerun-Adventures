using UnityEditor;
using UnityEngine;

/// <summary>
/// by ChatGPT
/// </summary>

[CustomPropertyDrawer(typeof(Skill))]
public class SkillDrawer : PropertyDrawer {
    //private Character character; // Reference to the Character component

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // Get the serialized object
        SerializedObject serializedObject = property.serializedObject;
        Skill skill = fieldInfo.GetValue(property.serializedObject.targetObject) as Skill;
        SerializedProperty skillTypeProperty = property.FindPropertyRelative("skillType");
        SerializedProperty abilityTypeProperty = property.FindPropertyRelative("abilityType");
        SerializedProperty proficiencyProperty = property.FindPropertyRelative("proficiency");
        SerializedProperty skillModifierProperty = property.FindPropertyRelative("SkillModifier");


        CharacterDataSO characterDataSO = property.serializedObject.targetObject as CharacterDataSO;

        // Calculate the rect for the foldout
        Rect foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);

        // Calculate the rect for the skill type enum (width is half of available space)
        Rect typeRect = new Rect(foldoutRect.xMax + 2f, position.y, (position.width - EditorGUIUtility.singleLineHeight * 2f - 10f) * 0.5f, EditorGUIUtility.singleLineHeight);

        // Calculate the rect for the proficiency checkbox
        Rect proficiencyRect = new Rect(typeRect.xMax + 5f, position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);

        // Draw the foldout
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GUIContent.none);

        // Draw the skill type enum
        EditorGUI.PropertyField(typeRect, skillTypeProperty, GUIContent.none);

        // Draw the proficiency checkbox
        EditorGUI.PropertyField(proficiencyRect, proficiencyProperty, GUIContent.none);


        if (property.isExpanded) {
            // Indent the content within the property field
            EditorGUI.indentLevel++;

            // Calculate the rect for the base ability label
            Rect baseAbilityLabelRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

            // Calculate the rect for the ability type
            Rect abilityTypeRect = new Rect(baseAbilityLabelRect.xMax, baseAbilityLabelRect.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

            // Draw the base ability label
            EditorGUI.LabelField(baseAbilityLabelRect, "Base Ability:");


            string abilityType = ((Ability.AbilityType)abilityTypeProperty.enumValueIndex).ToString();

            //EditorGUI.LabelField(modifierRect, "Mod: " + ((Ability.AbilityType)typeProperty.enumValueIndex).ToString());

            // Draw the ability type as a label
            EditorGUI.LabelField(abilityTypeRect, abilityType);

            // Calculate the rect for the modifier label
            Rect modifierLabelRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

            // Calculate the rect for the modifier value
            Rect modifierValueRect = new Rect(modifierLabelRect.xMax, modifierLabelRect.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

            // Draw the modifier label
            EditorGUI.LabelField(modifierLabelRect, "Modifier:");

            // Calculate the modifier
            int modifier = characterDataSO != null ? characterDataSO.GetSkillModifier((Skill.SkillType)skillTypeProperty.enumValueIndex) : 888;

            // Draw the modifier value
            EditorGUI.LabelField(modifierValueRect, modifier.ToString());

            // Reduce the indent level
            EditorGUI.indentLevel--;
        }

        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        float baseHeight = EditorGUI.GetPropertyHeight(property, label, true);

        if (property.isExpanded) {
            // Add extra height for the expanded content
            return baseHeight;
        }

        return baseHeight;
    }
}
