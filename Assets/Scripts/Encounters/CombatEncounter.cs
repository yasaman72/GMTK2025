using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Encounter_Combat_A00_00", menuName = "ScriptableObjects/Encounters/Combat Encounter")]
    public class CombatEncounter : BaseEncounter
    {
        [SerializeField] private int _numberOfEnemiesToSpawn = 1;
        [SerializeField] private Enemy[] _enemyTypes;

        public override void StartEncounter()
        {
            CombatTargetSelection.SetTargetAction?.Invoke(null);
            CombatManager.OnCombatStartEvent?.Invoke(_numberOfEnemiesToSpawn, _enemyTypes);
            CombatManager.OnCombatFinishedEvent += FinishEncounter;
        }

        public override void FinishEncounter()
        {
            CombatTargetSelection.SetTargetAction?.Invoke(null);
            CombatManager.OnCombatFinishedEvent -= FinishEncounter;
            EncounterManager.OnEncounterFinished?.Invoke();
        }
    }
}
