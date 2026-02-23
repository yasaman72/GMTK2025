
using System;
using UnityEngine;

namespace Deviloop
{
    [Flags]
    public enum F_PassageOptionType
    {
        Rest = 1 << 0,
        Soil = 1 << 1,
        Blacksmith = 1 << 2,
        Chest = 1 << 3,
    }

    public enum PassageOptionType
    {
        Rest = F_PassageOptionType.Rest,
        Soil = F_PassageOptionType.Soil,
        Blacksmith = F_PassageOptionType.Blacksmith,
        Chest = F_PassageOptionType.Chest,
    }

    [CreateAssetMenu(fileName = "Encounter_Passage_A00_00", menuName = "Scriptable Objects/Encounters/Passage")]
    public class PassageEncounter : BaseEncounter
    {
        public F_PassageOptionType config;

        public override void ResetEncounter()
        {
            PassageManager.OnEncounterFinished -= FinishEncounter;

        }

        public override void StartEncounter()
        {
            PassageManager.OnPassageOpenEvent?.Invoke(config);
            PassageManager.OnEncounterFinished += FinishEncounter;

        }

        public override void FinishEncounter()
        {
            PassageManager.OnEncounterFinished -= FinishEncounter;
            EncounterManager.OnEncounterFinished?.Invoke();
        }
    }
}
