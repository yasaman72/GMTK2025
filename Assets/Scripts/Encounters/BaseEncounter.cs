
using UnityEngine;

public abstract class BaseEncounter : ScriptableObject
{
    public string EncounterName;
    public Sprite EncounterIcon;
    public abstract void StartEncounter();
    public abstract void FinishEncounter();
}
