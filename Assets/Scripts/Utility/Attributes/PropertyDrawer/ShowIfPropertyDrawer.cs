using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ShouldShow(property)
            ? EditorGUI.GetPropertyHeight(property, label, true)
            : 0f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!ShouldShow(property))
            return;

        EditorGUI.PropertyField(position, property, label, true);
    }

    private bool ShouldShow(SerializedProperty property)
    {
        var showIf = (ShowIfAttribute)attribute;

        SerializedProperty conditionProperty = FindRelativeProperty(property, showIf.ConditionField);

        if (conditionProperty == null)
        {
            Debug.LogWarning(
                $"ShowIf: Could not find field '{showIf.ConditionField}' relative to '{property.propertyPath}'");
            return true;
        }

        if (conditionProperty.propertyType != SerializedPropertyType.Boolean)
        {
            Debug.LogWarning(
                $"ShowIf: Field '{showIf.ConditionField}' is not a bool");
            return true;
        }

        bool value = conditionProperty.boolValue;
        return showIf.Invert ? !value : value;
    }

    private SerializedProperty FindRelativeProperty(SerializedProperty property, string fieldName)
    {
        string path = property.propertyPath;
        int lastDot = path.LastIndexOf('.');

        if (lastDot < 0)
            return property.serializedObject.FindProperty(fieldName);

        string conditionPath = path.Substring(0, lastDot) + "." + fieldName;
        return property.serializedObject.FindProperty(conditionPath);
    }
}
#endif