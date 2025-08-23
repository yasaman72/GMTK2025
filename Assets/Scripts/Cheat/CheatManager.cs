using UnityEngine;
using static LootSet;

public class CheatManager : MonoBehaviour
{
    [SerializeField] GameObject _shop;
    [SerializeField] GameObject _enemies;
    [Header("logging")]
    [SerializeField] TMPro.TextMeshProUGUI _logText;
    [SerializeField] int maxLines = 50;

    private readonly System.Collections.Generic.Queue<string> logQueue = new();

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedLog = $"[{type}] {logString}";
        if (type == LogType.Error || type == LogType.Exception)
            formattedLog += $"\n{stackTrace}";

        logQueue.Enqueue(formattedLog);

        while (logQueue.Count > maxLines)
            logQueue.Dequeue();

        string combinedLogs = string.Join("\n", logQueue);

        if (_logText != null)
            _logText.text = combinedLogs;
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
}
