using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using Random = Deviloop.SeededRandom;

namespace Deviloop
{
    public class CombatManager : Singleton<CombatManager>
    {
        // argument is the number of enemies to spawn
        public static Action<EnemyType[]> OnCombatStartEvent;
        public static Action OnCombatFinishedEvent;
        public static Action OnAfterAllEnemiesDefeated;

        [SerializeField] private Transform _enemySpawnCenter;
        [SerializeField] private float _enemySpawnAreaWidth = 5;
        private static List<Enemy> _spawnedEnemies = new List<Enemy>();
        private static List<Enemy> _defeatedEnemies = new List<Enemy>();
        public static List<Enemy> SpawnedEnemies => _spawnedEnemies;


        private static int _combatRoundCounter = 0;
        public static int CombatRoundCounter
        {
            get => _combatRoundCounter;

            private set
            {
                _combatRoundCounter = value;
                CombatRoundCounterVariable.Value = _combatRoundCounter;
            }
        }
        private static IntVariable CombatRoundCounterVariable;
        private static BoolVariable IsInCombatVariable;

        private void Awake()
        {
            var source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
            IsInCombatVariable = source["global"]["IsInCombat"] as BoolVariable;
            IsInCombatVariable.Value = false;

            CombatRoundCounterVariable = source["global"]["CurrentCombatRoundCounter"] as IntVariable;
            CombatRoundCounterVariable.Value = 0;

            OnCombatStartEvent += StartCombat;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnCombatStartEvent -= StartCombat;
        }

        private void OnEnable()
        {
            TurnManager.OnTurnChanged += HandleTurnChanged;
        }

        private void OnDisable()
        {
            TurnManager.OnTurnChanged -= HandleTurnChanged;
            _defeatedEnemies.Clear();
            SpawnedEnemies.Clear();
        }

        private async void HandleTurnChanged(TurnManager.ETurnMode mode)
        {
            if (mode == TurnManager.ETurnMode.Player)
            {
                CombatRoundCounter++;
                return;
            }

            List<Action> enemyActionTasks = new List<Action>();
            var spawnedEnemiesCopy = SpawnedEnemies.ToList();
            foreach (var enemy in spawnedEnemiesCopy)
            {
                if (enemy == null || enemy.IsDead())
                    continue;

                // TODO: can use a initiative system later on for the order of actions
                await enemy.TakeNextActionAsync();
                enemyActionTasks.Add(() => enemy.PickNextAction());
            }

            foreach (var action in enemyActionTasks)
            {
                action.Invoke();
            }

            // after all enemies have taken their action, change the turn back to the player
            TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
        }

        public void StartCombat(EnemyType[] enemyTypes)
        {
            IsInCombatVariable.Value = true;

            CombatRoundCounter = 0;
            DestroyCurrentEnemies();

            gameObject.SetActive(true);

            SpawnEnemies(enemyTypes);

            CombatTargetSelection.SetTargetAction?.Invoke(SpawnedEnemies[0]);
        }

        private async Task SpawnEnemies(EnemyType[] enemyTypes)
        {
            int numberOfEnemiesToSpawn = enemyTypes.Sum(et => et.Quantity);

            float spacing = _enemySpawnAreaWidth / Mathf.Max(1, numberOfEnemiesToSpawn);
            Vector2 center = _enemySpawnCenter.position;

            float totalWidth = spacing * (numberOfEnemiesToSpawn - 1);
            Vector2 startPos = center - new Vector2(totalWidth / 2f, 0);

            int i = -1;
            ModifiableFloat spawnDelay = new ModifiableFloat(0.3f);
            foreach (var enemyType in enemyTypes)
            {
                for (int j = 0; j < enemyType.Quantity; j++)
                {
                    Vector2 spawnPosition = startPos + new Vector2(++i * spacing, Random.Range(-.5f, .5f));
                    SpawnNewEnemy(enemyType, spawnPosition);
                    await Awaitable.WaitForSecondsAsync(spawnDelay.Value);
                }
            }
            TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
        }

        private void DestroyCurrentEnemies()
        {
            var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            ObjectPool<Enemy> pool = null;
            foreach (var enemy in enemies)
            {
                pool = PoolManager.Instance.GetPool(enemy);
                pool.ReturnToPool(enemy);
            }
        }

        private void SpawnNewEnemy(EnemyType enemyToSpawn, Vector2 spawnPosition)
        {
            ObjectPool<Enemy> pool = null;
            try
            {
                pool = PoolManager.Instance.GetPool(enemyToSpawn.EnemyData.prefab);
            }
            catch (Exception)
            {
                pool = PoolManager.Instance.CreatePool(enemyToSpawn.EnemyData.prefab, 3, true);
            }

            Enemy newEnemy = pool.Get();
            GameObject newEnemyObj = newEnemy.gameObject;
            newEnemyObj.transform.position = spawnPosition;
            newEnemyObj.transform.rotation = Quaternion.identity;
            newEnemy.Stats = enemyToSpawn.EnemyData;
            SpawnedEnemies.Add(newEnemy);
            newEnemy.OnDeath += HandleEnemyDeath;
        }

        private void HandleEnemyDeath(CombatCharacter combatCharacter)
        {
            combatCharacter.OnDeath -= HandleEnemyDeath;

            _defeatedEnemies.Add(combatCharacter as Enemy);

            if (_defeatedEnemies.Count >= SpawnedEnemies.Count)
            {
                AfterAllEnemiesDefeated();
            }

            AnalyticsManager.SendCustomEventAction?.Invoke("enemy_defeated", new Dictionary<string, object>
        {
            { "enemy_type", combatCharacter.Stats.name }
        });
        }

        public void AfterAllEnemiesDefeated()
        {
            IsInCombatVariable.Value = false;
            SpawnedEnemies.Clear();
            _defeatedEnemies.Clear();

            RelicManager.ApplyEffectsForEvent<OnCombatEndEvent>(this);
            OnAfterAllEnemiesDefeated?.Invoke();

            ShowRewards();
        }

        private void ShowRewards()
        {
            if (EncounterManager.CurrentEncounter is CombatEncounter combatEncounter)
            {

                RewardView.OpenRewards?.Invoke(combatEncounter.DefeatRewards);
                RewardView.OnRewardsClosed += FinishCombat;
            }
            else if (EncounterManager.CurrentEncounter is EnemyWaveEncounter waveEncounter)
            {
                if (waveEncounter.AreWavesFinished())
                {
                    RewardView.OpenRewards?.Invoke(waveEncounter.DefeatRewards);
                    RewardView.OnRewardsClosed += FinishCombat;
                }
                else
                {
                    _defeatedEnemies.Clear();
                    SpawnedEnemies.Clear();
                    OnCombatFinishedEvent?.Invoke();
                }
            }
            else
            {
                Debug.LogError("Current encounter is not a CombatEncounter. Cannot show rewards.");
                FinishCombat();
            }
        }

        private void FinishCombat()
        {
            RewardView.OnRewardsClosed -= FinishCombat;

            gameObject.SetActive(false);
            OnCombatFinishedEvent?.Invoke();
        }

        public Enemy GetRandomEnemy()
        {
            List<Enemy> aliveEnemies = SpawnedEnemies.Where(e => !e.IsDead()).ToList();
            return ListUtilities.GetRandomElement(aliveEnemies);
        }

        public bool IsAnyEnemyAlive()
        {
            return SpawnedEnemies.Any(e => !e.IsDead());
        }
    }
}
