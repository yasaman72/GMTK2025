using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemySpawnPos;
    [SerializeField] private float _waitBeforeNewEnemySpawn = 3;

    private GameObject _currentEnemy;

    private void Start()
    {
        var enemies = FindObjectsByType<EnemyBrain>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        SpawnNewEnemy();
    }

    private void SpawnNewEnemy()
    {
        _currentEnemy = Instantiate(_enemyPrefab, _enemySpawnPos.position, Quaternion.identity, _enemySpawnPos);
        _currentEnemy.GetComponent<EnemyBrain>().OnEnemyDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        _currentEnemy.GetComponent<EnemyBrain>().OnEnemyDeath -= HandleEnemyDeath;
        StartCoroutine(EnemyDeathAndRespawn());
    }

    IEnumerator EnemyDeathAndRespawn()
    {
        yield return new WaitForSeconds(_waitBeforeNewEnemySpawn);
        Destroy(_currentEnemy);
        SpawnNewEnemy();
    }
}
