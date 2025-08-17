using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
public class MinMaxSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Vector2)
        {
            EditorGUI.LabelField(position, label.text, "Use MinMaxSlider with Vector2");
            return;
        }

        MinMaxSliderAttribute range = (MinMaxSliderAttribute)attribute;
        Vector2 value = property.vector2Value;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 2f;

        // Label
        Rect labelRect = new Rect(position.x, position.y, position.width, lineHeight);
        EditorGUI.LabelField(labelRect, label);

        // Min/Max float fields + slider
        Rect fieldsRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);
        float fieldWidth = 50f;
        Rect minFieldRect = new Rect(fieldsRect.x, fieldsRect.y, fieldWidth, lineHeight);
        Rect maxFieldRect = new Rect(fieldsRect.xMax - fieldWidth, fieldsRect.y, fieldWidth, lineHeight);
        Rect sliderRect = new Rect(minFieldRect.xMax + 4, fieldsRect.y, fieldsRect.width - (fieldWidth * 2 + 8) - 50, lineHeight);

        // Reset button Rect
        Rect resetButtonRect = new Rect(sliderRect.xMax + 4, fieldsRect.y, 50f, lineHeight);

        // Begin change tracking
        EditorGUI.BeginChangeCheck();

        // Editable Min/Max fields
        value.x = EditorGUI.FloatField(minFieldRect, value.x);
        value.y = EditorGUI.FloatField(maxFieldRect, value.y);

        // MinMaxSlider
        EditorGUI.MinMaxSlider(sliderRect, ref value.x, ref value.y, range.Min, range.Max);

        // Clamp values
        value.x = Mathf.Clamp(value.x, range.Min, value.y);
        value.y = Mathf.Clamp(value.y, value.x, range.Max);

        // Reset button functionality
        if (GUI.Button(resetButtonRect, "Reset"))
        {
            value = Vector2.one;
        }

        // Apply changes if any
        if (EditorGUI.EndChangeCheck())
        {
            property.vector2Value = value;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + 2;
    }
}

#endif
