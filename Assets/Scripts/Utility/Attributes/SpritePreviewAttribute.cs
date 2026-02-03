using UnityEngine;

public class SpritePreviewAttribute : PropertyAttribute
{
    public readonly float PreviewSize;

    public SpritePreviewAttribute(float previewSize = 64f)
    {
        PreviewSize = previewSize;
    }
}
