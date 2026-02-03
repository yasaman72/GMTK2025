using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpritePreviewAttribute))]
public class SpritePreviewDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = (SpritePreviewAttribute)attribute;

        float baseHeight = EditorGUIUtility.singleLineHeight;

        if (property.objectReferenceValue == null)
            return baseHeight;

        return baseHeight + attr.PreviewSize + EditorGUIUtility.standardVerticalSpacing;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (SpritePreviewAttribute)attribute;

        Rect fieldRect = new Rect(
            position.x,
            position.y,
            position.width,
            EditorGUIUtility.singleLineHeight
        );

        EditorGUI.PropertyField(fieldRect, property, label);

        if (property.objectReferenceValue == null)
            return;

        Sprite sprite = property.objectReferenceValue as Sprite;
        if (sprite == null)
            return;

        Texture2D texture = sprite.texture;
        Rect texRect = sprite.textureRect;

        Rect previewRect = new Rect(
            position.x,
            fieldRect.yMax + EditorGUIUtility.standardVerticalSpacing,
            attr.PreviewSize,
            attr.PreviewSize
        );

        float aspect = texRect.width / texRect.height;
        if (aspect > 1f)
            previewRect.height /= aspect;
        else
            previewRect.width *= aspect;

        GUI.DrawTextureWithTexCoords(
            previewRect,
            texture,
            new Rect(
                texRect.x / texture.width,
                texRect.y / texture.height,
                texRect.width / texture.width,
                texRect.height / texture.height
            )
        );
    }
}
