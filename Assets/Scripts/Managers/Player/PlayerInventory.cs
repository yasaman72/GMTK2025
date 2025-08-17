using System;
using UnityEngine;
using static LootSet;

public class PlayerInventory : CustomMonoBehavior
{
    public static Action<LootSetData> AddToInventoryAction;

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

    public static bool SpendCoin(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("Attempted to spend a non-positive amount of coins.");
            return false;
        }
        if (CoinCount < amount)
        {
            Debug.Log($"Not enough coins to spend. Current coins: {CoinCount}, Attempted to spend: {amount}");
            return true;
        }
        CoinCount -= amount;
        return true;
    }
}
