
using UnityEngine;

public abstract class BaseEncounter : ScriptableObject
{
    //TODO: replace with localized string
    public string EncounterName;
    public Sprite EncounterIcon;
    public abstract void StartEncounter();
    public abstract void FinishEncounter();
}
