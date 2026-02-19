using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public struct EnemyType
    {
        public EnemyData EnemyData;
        public int Quantity;
    }

    [CreateAssetMenu(fileName = "Encounter_Combat_A00_00", menuName = "Scriptable Objects/Encounters/Combat Encounter")]
    public class CombatEncounter : BaseEncounter
    {
        [SerializeField] public EnemyType[] enemyTypes;
        public bool IsElite = false;
        public List<LootSet> DefeatRewards;

        public override void ResetEncounter()
        {
            CombatManager.OnCombatFinishedEvent -= FinishEncounter;
        }

        public override void StartEncounter()
        {
            CombatTargetSelection.SetTargetAction?.Invoke(null);
            CombatManager.OnCombatStartEvent?.Invoke(enemyTypes);
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
