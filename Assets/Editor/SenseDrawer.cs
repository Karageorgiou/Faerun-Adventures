// Custom property drawer for the Sense class
using static Sense;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Sense))]
public class SenseDrawer : PropertyDrawer {
    private const float TypeWidth = 100f;
    private const float RangeWidth = 30f;
    private const float LabelWidth = 50f;
    private const float Padding = 5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        // Get the senseType property
        SerializedProperty senseTypeProperty = property.FindPropertyRelative("senseType");

        // Calculate the rect for the senseType dropdown field
        Rect typeRect = new Rect(position.x, position.y, TypeWidth, position.height);

        // Calculate the rect for the senseRange field
        Rect rangeRect = new Rect(position.x + position.width - RangeWidth, position.y, RangeWidth, position.height);

        // Draw the senseType dropdown field
        EditorGUI.PropertyField(typeRect, senseTypeProperty, GUIContent.none);

        // Draw the label for the range field
        Rect labelRect = new Rect(position.x + position.width - RangeWidth - LabelWidth, position.y, position.width - TypeWidth - RangeWidth - Padding, position.height);
        EditorGUI.LabelField(labelRect, "Range:");

        // Get the senseRange property
        SerializedProperty senseRangeProperty = property.FindPropertyRelative("senseRange");

        // Draw the senseRange field
        EditorGUI.PropertyField(rangeRect, senseRangeProperty, GUIContent.none);

        EditorGUI.EndProperty();
    }


}