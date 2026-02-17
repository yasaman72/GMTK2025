using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyWaveData", menuName = "Scriptable Objects/EnemyWaveData")]
    public class EnemyWaveData : BaseEncounter
    {
        public List<CombatEncounter> encounters;
        public bool IsElite = false;
        public List<LootSet> DefeatRewards;

        private int _lastEncounterIndex = -1;

        public override void StartEncounter()
        {
            _lastEncounterIndex = -1;
            GoToNextEncounter();
            CombatManager.OnCombatFinishedEvent += FinishEncounter;
        }

        public override void FinishEncounter()
        {
            if (_lastEncounterIndex >= encounters.Count - 1)
            {
                CombatManager.OnCombatFinishedEvent -= FinishEncounter;
                CombatTargetSelection.SetTargetAction?.Invoke(null);
                EncounterManager.OnEncounterFinished?.Invoke();
                _lastEncounterIndex = -1;
                return;
            }

            CombatTargetSelection.SetTargetAction?.Invoke(null);
            GoToNextEncounter();
        }

        private void GoToNextEncounter()
        {
            _lastEncounterIndex++;

            CombatTargetSelection.SetTargetAction?.Invoke(null);
            CombatManager.OnCombatStartEvent?.Invoke(encounters[_lastEncounterIndex].enemyTypes);
            TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
        }

        public bool AreWavesFinished()
        {
            return _lastEncounterIndex >= encounters.Count - 1;
        }
    }
}
