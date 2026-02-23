using Deviloop;
using System;
using static LootSet;

public class PlayerInventory : CustomMonoBehavior
{
    public static Action<LootSetData> AddToInventoryAction;
    public static Action OnNotEnoughGold;

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
        if (loot.IsCoinLoot())
        {
            CoinCount += loot.Count;
        }
        else if (loot.item is CardLoot cardLoot)
        {
            CardManager.AddCardToDeckAction?.Invoke(cardLoot.Card, 1);
        }
        else if (loot.item is RelicLoot relicLoot && relicLoot.Relic != null)
        {
            RelicManager.AddRelic(relicLoot.Relic);
        }
        else if (loot.item is MaterialLoot materialLoot && materialLoot.materialType != null)
        {
            Logger.Log($"adding x{loot.Count} {materialLoot.itemName}.");
        }
        else
        {
            Logger.LogWarning("Attempted to add a non-coin loot item to the inventory.");
        }
    }

    public static bool SpendCoin(int amount)
    {
        if (amount <= 0)
        {
            Logger.LogWarning("Attempted to spend a non-positive amount of coins.");
            return false;
        }
        if (CoinCount < amount)
        {
            Logger.Log($"Not enough coins to spend. Current coins: {CoinCount}, Attempted to spend: {amount}");
            OnNotEnoughGold?.Invoke();
            return false;
        }
        CoinCount -= amount;
        return true;
    }
}
