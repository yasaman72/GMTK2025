using Deviloop;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemySpawnPos;
    [SerializeField] private float _waitBeforeNewEnemySpawn = 3;
    [SerializeField] private TextMeshProUGUI _deatedEnemiesCounter;

    private GameObject _currentEnemy;
    private EnemyStats _currentEnemyStats;
    private int _defeatedEnemiesCount = 0;

    private void Start()
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        _deatedEnemiesCounter.text = "0";

        SpawnNewEnemy();
    }

    private void SpawnNewEnemy()
    {
        _currentEnemy = Instantiate(_enemyPrefab, _enemySpawnPos.position, Quaternion.identity, _enemySpawnPos);
        _currentEnemy.GetComponent<Enemy>().OnDeath += HandleEnemyDeath;
        _currentEnemyStats = _currentEnemy.GetComponent<Enemy>().Stats as EnemyStats;
    }

    private void HandleEnemyDeath()
    {
        _currentEnemy.GetComponent<Enemy>().OnDeath -= HandleEnemyDeath;
        StartCoroutine(EnemyDeathAndRespawn());

        _defeatedEnemiesCount++;
        _deatedEnemiesCounter.text = _defeatedEnemiesCount.ToString();

        AnalyticsManager.SendCustomEventAction?.Invoke("enemy_defeated", new System.Collections.Generic.Dictionary<string, object>
        {
            { "enemy_type", _currentEnemyStats.name }
        });
    }

    IEnumerator EnemyDeathAndRespawn()
    {
        yield return new WaitForSeconds(_waitBeforeNewEnemySpawn);
        RewardView.OpenRewards?.Invoke(_currentEnemyStats.defeatRewards);

        Destroy(_currentEnemy);
        SpawnNewEnemy();
    }
}
