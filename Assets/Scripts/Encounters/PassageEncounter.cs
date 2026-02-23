
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Encounter_Passage_A00_00", menuName = "Scriptable Objects/Encounters/Passage")]
    public class PassageEncounter : BaseEncounter
    {
        public override void ResetEncounter()
        {
            PassageManager.OnEncounterFinished -= FinishEncounter;

        }

        public override void StartEncounter()
        {
            PassageManager.OnPassageOpenEvent?.Invoke();
            PassageManager.OnEncounterFinished += FinishEncounter;

        }

        public override void FinishEncounter()
        {
            PassageManager.OnEncounterFinished -= FinishEncounter;
            EncounterManager.OnEncounterFinished?.Invoke();
        }
    }
}
