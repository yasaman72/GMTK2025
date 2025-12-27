using Deviloop;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnityEngine.Object), true)]
public class DeveloperNotesInspector : Editor
{
    private List<SerializedProperty> _normalProps = new();
    private SerializedProperty _notesProp;
    private DeveloperNotesAttribute _noteSettings;

    private GUIStyle _noteBox;
    private GUIStyle _noteHeader;

    private void OnEnable()
    {
        _normalProps.Clear();
        _notesProp = null;

        var type = target.GetType();

        while (type != null && type != typeof(object))
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

            foreach (var field in type.GetFields(flags))
            {
                // Respect Unity's HideInInspector
                if (field.IsDefined(typeof(HideInInspector), true))
                    continue;

                // Respect serialization rules
                if (!field.IsPublic && !field.IsDefined(typeof(SerializeField), true))
                    continue;

                var prop = serializedObject.FindProperty(field.Name);
                if (prop == null) continue;

                var designerAttr = field.GetCustomAttribute<DeveloperNotesAttribute>(true);

                if (designerAttr != null)
                {
                    _notesProp = prop;
                    _noteSettings = designerAttr;
                }
                else if (!_normalProps.Exists(p => p.name == prop.name))
                    _normalProps.Add(prop);
            }

            type = type.BaseType;
        }
    }


    public override void OnInspectorGUI()
    {
        if (_noteBox == null)
        {
            _noteBox = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(10, 10, 8, 10)
            };

            _noteHeader = new GUIStyle(EditorStyles.boldLabel);
            _noteHeader.normal.textColor = new Color(1f, 0.55f, 0.2f);
        }

        serializedObject.Update();

        foreach (var prop in _normalProps)
            EditorGUILayout.PropertyField(prop, true);

        if (_notesProp != null)
        {
            GUILayout.Space(_noteSettings.SpaceBefore);
            EditorGUILayout.BeginVertical(_noteBox);
            EditorGUILayout.LabelField("⚠ DEVELOPER NOTES", _noteHeader);
            if (_noteSettings != null)
            {
                var textAreaStyle = new GUIStyle(EditorStyles.textArea)
                {
                    wordWrap = true
                };

                _notesProp.stringValue = EditorGUILayout.TextArea(
                    _notesProp.stringValue,
                    textAreaStyle,
                    GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * _noteSettings.MinLines),
                    GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight * _noteSettings.MaxLines)
                );
            }
            else
            {
                EditorGUILayout.PropertyField(_notesProp, GUIContent.none, true);
            }
            EditorGUILayout.EndVertical();
        }
        else
        {
            GUILayout.Space(15);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
