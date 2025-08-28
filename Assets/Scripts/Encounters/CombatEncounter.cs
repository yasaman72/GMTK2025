using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Encounter_Combat_A00_00", menuName = "ScriptableObjects/Encounters/Combat Encounter")]
    public class CombatEncounter : BaseEncounter
    {
        //TODO: Add combat specific data here
        //[SerializeField] private EncounterData _encounterData;

        public override void StartEncounter()
        {
            CombatManager.OnCombatStartEvent?.Invoke();
            CombatManager.OnCombatFinishedEvent += FinishEncounter;
        }

        public override void FinishEncounter()
        {
            CombatManager.OnCombatFinishedEvent -= FinishEncounter;
            EncounterManager.OnEncounterFinished?.Invoke();
        }
    }
}
