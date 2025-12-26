using Deviloop;
using FMODUnity;
using UnityEngine;

public abstract class LootItem: ScriptableObject
{
    public string itemName;
    [TextArea(3, 10)]
    public string description;
    public Sprite icon;
    public EventReference OnLootSound;

    [DeveloperNotes, SerializeField]
    private string developerNotes;
}