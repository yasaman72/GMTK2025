using System;
using UnityEngine;
using static LootSet;

public class PlayerInventory : MonoBehaviour
{
    public static Action<LootSetData> AddToInventoryAction;

    [SerializeField] private bool shouldLog = false;

    private static int coinCount;
    public static int CoinCount
    {
        get { return coinCount; }
        set
        {
            coinCount = value;
            PlayerInventoryUI.Update?.Invoke();
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        CoinCount = 0;
    }

    private void OnEnable()
    {
        AddToInventoryAction += Add;
    }

    private void OnDisable()
    {
        AddToInventoryAction -= Add;
    }

    private void Add(LootSetData loot)
    {
        if (loot.Item is CoinLoot coinLoot)
        {
            CoinCount += loot.Count;
            Logger.Log($"Added {loot.Count} coins to inventory. Total coins: {CoinCount}", shouldLog);
        }
        else
        {
            Logger.Log("Attempted to add a non-coin loot item to the inventory.", shouldLog);
        }
    }
}
