using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIfAttribute = (ShowIfAttribute)attribute;

        // Find the boolean field that will control the visibility of this field
        SerializedProperty booleanField = property.serializedObject.FindProperty(showIfAttribute.BooleanFieldName);

        if (booleanField != null)
        {
            if (booleanField.boolValue)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
        else
        {
            Debug.LogWarning("Could not find boolean field: " + showIfAttribute.BooleanFieldName);
            EditorGUI.PropertyField(position, property, label);
        }
    }
}

#endif
