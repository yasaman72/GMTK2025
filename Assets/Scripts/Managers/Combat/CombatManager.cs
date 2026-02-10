using Deviloop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using Random = Deviloop.SeededRandom;

namespace Deviloop
{
    public class CombatManager : MonoBehaviour
    {
        // argument is the number of enemies to spawn
        public static Action<EnemyType[]> OnCombatStartEvent;
        public static Action OnCombatFinishedEvent;
        public static Action OnAfterAllEnemiesDefeated;

        [SerializeField] private Transform _enemySpawnCenter;
        [SerializeField] private float _waitBeforeShowingRewards = 3;
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
            _spawnedEnemies.Clear();
        }

        private void HandleTurnChanged(TurnManager.ETurnMode mode)
        {
            if (mode == TurnManager.ETurnMode.Player)
            {
                CombatRoundCounter++;
            }
        }

        public void StartCombat(EnemyType[] enemyTypes)
        {
            IsInCombatVariable.Value = true;

            CombatRoundCounter = 0;
            DestroyCurrentEnemies();

            gameObject.SetActive(true);

            StartCoroutine(SpawnEnemies(enemyTypes));

            CombatTargetSelection.SetTargetAction?.Invoke(_spawnedEnemies[0]);
        }

        private IEnumerator SpawnEnemies(EnemyType[] enemyTypes)
        {
            int numberOfEnemiesToSpawn = enemyTypes.Sum(et => et.Quantity);

            float spacing = _enemySpawnAreaWidth / Mathf.Max(1, numberOfEnemiesToSpawn);
            Vector2 center = _enemySpawnCenter.position;

            float totalWidth = spacing * (numberOfEnemiesToSpawn - 1);
            Vector2 startPos = center - new Vector2(totalWidth / 2f, 0);

            int i = -1;
            foreach (var enemyType in enemyTypes)
            {
                for (int j = 0; j < enemyType.Quantity; j++)
                {
                    Vector2 spawnPosition = startPos + new Vector2(++i * spacing, Random.Range(-.5f, .5f));
                    SpawnNewEnemy(enemyType, spawnPosition);
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }

        private void DestroyCurrentEnemies()
        {
            var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
        }

        private void SpawnNewEnemy(EnemyType enemyToSpawn, Vector2 spawnPosition)
        {
            GameObject newEnemyObj = Instantiate(enemyToSpawn.EnemyData.prefab, spawnPosition, Quaternion.identity, _enemySpawnCenter);
            Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();
            newEnemy.Stats = enemyToSpawn.EnemyData;
            _spawnedEnemies.Add(newEnemy);
            newEnemy.OnDeath += HandleEnemyDeath;
        }

        private void HandleEnemyDeath(CombatCharacter combatCharacter)
        {
            combatCharacter.OnDeath -= HandleEnemyDeath;

            _defeatedEnemies.Add(combatCharacter as Enemy);

            if (_defeatedEnemies.Count >= _spawnedEnemies.Count)
            {
                AfterAllEnemiesDefeated();
                StartCoroutine(ShowRewards(combatCharacter as Enemy));
            }

            AnalyticsManager.SendCustomEventAction?.Invoke("enemy_defeated", new Dictionary<string, object>
        {
            { "enemy_type", combatCharacter.Stats.name }
        });
        }

        private void AfterAllEnemiesDefeated()
        {
            IsInCombatVariable.Value = false;
            RelicManager.ApplyEffectsForEvent<OnCombatEndEvent>(this);
            OnAfterAllEnemiesDefeated?.Invoke();
        }

        private IEnumerator ShowRewards(Enemy enemy)
        {
            yield return new WaitForSeconds(_waitBeforeShowingRewards);

            if (EncounterManager.CurrentEncounter is CombatEncounter combatEncounter)
            {

                RewardView.OpenRewards?.Invoke(combatEncounter.DefeatRewards);
                RewardView.OnRewardsClosed += FinishCombat;
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

            _defeatedEnemies.Clear();
            _spawnedEnemies.Clear();

            gameObject.SetActive(false);
            OnCombatFinishedEvent?.Invoke();
        }
    }
}
