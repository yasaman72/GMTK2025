using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyWaveData", menuName = "Scriptable Objects/EnemyWaveData")]
    public class EnemyWaveData : BaseEncounter
    {
        public delegate void EnemyWaveDataDelegate(int encountersCount);
        public static EnemyWaveDataDelegate OnWaveEncounterStarted;
        public delegate void EnemyWaveProgressDelegate(int currentIndex);
        public static EnemyWaveProgressDelegate OnWaveEncounterProgressed;
        public static Action OnWaveEncounterFinished;

        [SerializeField] private float _delayBetweenSpawns = 1;
        public List<CombatEncounter> encounters;
        public bool IsElite = false;
        public List<LootSet> DefeatRewards;

        private int _lastEncounterIndex = -1;
        private bool _isNewWave = false;

        private CancellationTokenSource _cancellationTokenSource;


        public override void ResetEncounter()
        {
            _lastEncounterIndex = -1;
            CombatManager.OnCombatFinishedEvent -= FinishEncounter;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public override void StartEncounter()
        {
            _lastEncounterIndex = -1;
            GoToNextEncounter();
            CombatManager.OnCombatFinishedEvent += FinishEncounter;

            OnWaveEncounterStarted?.Invoke(encounters.Count);
        }

        public override void FinishEncounter()
        {
            if (_lastEncounterIndex >= encounters.Count - 1)
            {
                CombatManager.OnCombatFinishedEvent -= FinishEncounter;
                CombatTargetSelection.SetTargetAction?.Invoke(null);
                EncounterManager.OnEncounterFinished?.Invoke();
                OnWaveEncounterFinished?.Invoke();

                _lastEncounterIndex = -1;
                return;
            }

            CombatTargetSelection.SetTargetAction?.Invoke(null);
            GoToNextEncounter();
            OnWaveEncounterProgressed?.Invoke(_lastEncounterIndex);
        }

        private async void GoToNextEncounter()
        {
            _isNewWave = true;
            _lastEncounterIndex++;

            CombatTargetSelection.SetTargetAction?.Invoke(null);

            if (_lastEncounterIndex != 0)
                await Awaitable.WaitForSecondsAsync(_delayBetweenSpawns, _cancellationTokenSource.Token);

            CombatManager.OnCombatStartEvent?.Invoke(
                encounters[_lastEncounterIndex].enemyTypes);

            TurnManager.OnTurnChanged += AfterPlayerTookFirstTurn;
            TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
        }

        private void AfterPlayerTookFirstTurn(TurnManager.ETurnMode mode)
        {
            if (mode == TurnManager.ETurnMode.Player)
            {
                TurnManager.OnTurnChanged -= AfterPlayerTookFirstTurn;
                _isNewWave = false;
            }
        }

        public bool NewWaveIsNewlySpawned()
        {
            if (_lastEncounterIndex == 0) 
                return false;

            return _isNewWave;
        }

        public bool AreWavesFinished()
        {
            return _lastEncounterIndex >= encounters.Count - 1;
        }
    }
}
