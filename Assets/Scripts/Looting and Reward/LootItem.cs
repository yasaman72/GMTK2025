using UnityEngine;

public abstract class LootItem: ScriptableObject
{
    public string ItemName;
    [TextArea(3, 10)]
    public string Description;
    public Sprite Icon;
}
