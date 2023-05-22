using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Ability))]
public class AbilityDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProperty = property.FindPropertyRelative("type");
        SerializedProperty scoreProperty = property.FindPropertyRelative("abilityScore");
        SerializedProperty proficiencyProperty = property.FindPropertyRelative("proficiency");

        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 50f;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

        Rect typeRect = new Rect(position.x + position.width * 0.1f, position.y, position.width * 1f - labelWidth, lineHeight);
        EditorGUI.PropertyField(typeRect, typeProperty, new GUIContent("Type"));

        Rect secondLineRect = new Rect(position.x, position.y + lineHeight + verticalSpacing, position.width, lineHeight);

        Rect scoreRect = new Rect(secondLineRect.x + secondLineRect.width * 0.1f, secondLineRect.y, secondLineRect.width * 0.4f, lineHeight);
        EditorGUI.PropertyField(scoreRect, scoreProperty, new GUIContent("Score"));

        Rect profRect = new Rect(secondLineRect.x + secondLineRect.width * 0.6f, secondLineRect.y, secondLineRect.width * 0.4f, lineHeight);
        EditorGUI.LabelField(profRect, "Prof:");

        Rect checkboxRect = new Rect(secondLineRect.x + secondLineRect.width * 0.8f, secondLineRect.y, secondLineRect.width * 0.07f, lineHeight);
        EditorGUI.PropertyField(checkboxRect, proficiencyProperty, GUIContent.none);

        // Draw Modifier
        Ability.AbilityType abilityType = (Ability.AbilityType)typeProperty.enumValueIndex;
        int modifier = CalculateModifier(scoreProperty.intValue);
        string modifierText = "+  " + modifier.ToString();
        if (modifier < 0) {
            modifier = Mathf.Abs(modifier);
            modifierText = "-  " + modifier.ToString();
        }
       

        GUIStyle modifierStyle = new GUIStyle(EditorStyles.largeLabel);
        modifierStyle.alignment = TextAnchor.MiddleLeft;
        modifierStyle.fontStyle = FontStyle.Bold;

        Rect modifierRect = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.LabelField(modifierRect, modifierText, modifierStyle);

        EditorGUIUtility.labelWidth = labelWidth;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;
        return 2 * (lineHeight + verticalSpacing);
    }

    private int CalculateModifier(int abilityScore) {
        int modifier = abilityScore - 10;
        modifier += modifier < 0 ? -1 : 0;
        modifier /= 2;
        return modifier;
    }
}



//Rect modifierRect = new Rect(position.x + position.width * 0.4f, position.y, position.width * 0.2f, EditorGUIUtility.singleLineHeight);
//EditorGUI.LabelField(modifierRect, "Mod: " + ((Ability.AbilityType)typeProperty.enumValueIndex).ToString());
