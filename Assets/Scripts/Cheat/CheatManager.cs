using Deviloop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LootSet;

public class CheatManager : MonoBehaviour
{
    [SerializeField] GameObject _shop;
    [SerializeField] GameObject _enemies;
    [Header("logging")]
    [SerializeField] TMPro.TextMeshProUGUI _logText;
    [SerializeField] TMPro.TextMeshProUGUI _logLassoShape;
    [SerializeField] int maxLines = 50;

    private readonly Queue<string> logQueue = new();
    private static readonly Queue<string> logBuffer = new();

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        PlayerLassoManager.OnLassoShapeRecognized += LogLassoShape;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        PlayerLassoManager.OnLassoShapeRecognized -= LogLassoShape;
    }

    private void LogLassoShape(LassoShape shape)
    {
        if (_logLassoShape != null)
        {
            _logLassoShape.text = shape.ToString();
        }
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedLog = $"[{type}] {logString}";
        if (type == LogType.Error || type == LogType.Exception)
        {
            formattedLog = $"<color=red>{formattedLog}</color>";
            formattedLog += $"\n{stackTrace}";
        }
        else if (type == LogType.Warning)
        {
            formattedLog = $"<color=yellow>{formattedLog}</color>";
        }

        logQueue.Enqueue(formattedLog);
        logBuffer.Enqueue(formattedLog);

        while (logQueue.Count > maxLines)
            logQueue.Dequeue();

        string combinedLogs = string.Join("\n", logQueue);

        if (_logText != null)
            _logText.text = combinedLogs;
    }

    public static Queue<string> GetLogs()
    {
        return logBuffer;
    }

    public void AddCoin(int amount)
    {
        PlayerInventory.CoinCount += amount;
    }

    public void AddLoot(LootSetData loot)
    {
        PlayerInventory.AddToInventoryAction?.Invoke(loot);
    }

    public void ToggleShop()
    {
        _shop.SetActive(!_shop.activeInHierarchy);
        _enemies.SetActive(!_shop.activeInHierarchy);
    }

    public void ToggleEnemies()
    {
        _enemies.SetActive(!_enemies.activeInHierarchy);
        _shop.SetActive(!_enemies.activeInHierarchy);
    }

    public void DamageAllEnemies(int damageAmount)
    {
        List<Enemy> enemies = CombatManager.SpawnedEnemies.ToList();

        foreach (var enemy in enemies)
        {
            Player.PlayerCombatCharacter.DealDamage(enemy, damageAmount, AttackType.Normal);
        }
    }

    public void FullyHealPlayer()
    {
        Player.PlayerCombatCharacter.FullyHeal();
    }

    public void ClearLogs()
    {
        logQueue.Clear();
        if (_logText != null)
            _logText.text = string.Empty;
    }
}
