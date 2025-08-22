using UnityEngine;
using static LootSet;

public class CheatManager : MonoBehaviour
{
    [SerializeField] GameObject _shop;
    [SerializeField] GameObject _enemies;

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
