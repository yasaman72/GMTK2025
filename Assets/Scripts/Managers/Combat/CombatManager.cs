using Deviloop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    // argument is the number of enemies to spawn
    public static Action<int, Enemy[]> OnCombatStartEvent;
    public static Action OnCombatFinishedEvent;

    [SerializeField] private Transform _enemySpawnCenter;
    [SerializeField] private float _waitBeforeShowingRewards = 3;
    [SerializeField] private TextMeshProUGUI _deatedEnemiesCounter;
    [SerializeField] private float _enemySpawnAreaWidth = 5;

    private static List<Enemy> _spawnedEnemies = new List<Enemy>();
    private static List<Enemy> _defeatedEnemies = new List<Enemy>();
    public static List<Enemy> SpawnedEnemies => _spawnedEnemies;

    public static int CombatRoundCounter = 0;

    private void Awake()
    {
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
    }

    private void HandleTurnChanged(TurnManager.ETurnMode mode)
    {
        if (mode == TurnManager.ETurnMode.Player)
        {
            CombatRoundCounter++;
        }
    }

    public void StartCombat(int numberOfEnemiesToSpawn, Enemy[] enemyTypes)
    {
        DestroyCurrentEnemies();
        _deatedEnemiesCounter.text = "0";

        gameObject.SetActive(true);

        float spacing = _enemySpawnAreaWidth / Mathf.Max(1, numberOfEnemiesToSpawn);
        Vector2 center = _enemySpawnCenter.position;

        float totalWidth = spacing * (numberOfEnemiesToSpawn - 1);
        Vector2 startPos = center - new Vector2(totalWidth / 2f, 0);

        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            int enemyTypeIndex = Random.Range(0, enemyTypes.Length);
            GameObject _enemyPrefab = enemyTypes[enemyTypeIndex].gameObject;
            Vector2 spawnPosition = startPos + new Vector2(i * spacing, Random.Range(-.5f, .5f));

            SpawnNewEnemy(_enemyPrefab, spawnPosition);
        }

        CombatTargetSelection.SetTargetAction?.Invoke(_spawnedEnemies[0]);
    }

    private void DestroyCurrentEnemies()
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    private void SpawnNewEnemy(GameObject enemyToSpawn, Vector2 spawnPosition)
    {
        GameObject newEnemyObj = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, _enemySpawnCenter);
        Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();
        _spawnedEnemies.Add(newEnemy);
        newEnemy.OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath(CombatCharacter combatCharacter)
    {
        combatCharacter.OnDeath -= HandleEnemyDeath;

        _defeatedEnemies.Add(combatCharacter as Enemy);
        _deatedEnemiesCounter.text = _defeatedEnemies.Count.ToString();

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
        ApplyOnCombatEndRelicEffects();
    }

    private IEnumerator ShowRewards(Enemy enemy)
    {
        yield return new WaitForSeconds(_waitBeforeShowingRewards);
        RewardView.OpenRewards?.Invoke(_defeatedEnemies.Select(enemy => enemy.enemyStats.defeatRewards).ToList());
        RewardView.OnRewardsClosed += FinishCombat;
    }

    private void FinishCombat()
    {
        RewardView.OnRewardsClosed -= FinishCombat;

        _defeatedEnemies.Clear();
        _spawnedEnemies.Clear();
        _deatedEnemiesCounter.text = "";

        gameObject.SetActive(false);
        OnCombatFinishedEvent?.Invoke();
    }

    private void ApplyOnCombatEndRelicEffects()
    {
        var effects = RelicManager.GetEffectsForEvent<OnCombatEndEvent>();
        foreach (var effect in effects)
        {
            effect.Apply(this);
        }
    }
}
