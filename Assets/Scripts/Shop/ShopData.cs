using Cards;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Scriptable Objects/ShopData")]
public class ShopData : ScriptableObject
{
    public List<ShopItem> Items;
    public int itemDeletionPrice = 20;

    [System.Serializable]
    public struct ShopItem
    {
        public CardEntry CardEntry;
        public int Price;
    }

    public ShopData Copy()
    {
        var copy = CreateInstance(nameof(ShopData)) as ShopData;
        var _shopOffers = new List<ShopItem>();
        foreach (var item in this.Items)
        {
            var newItem = new ShopItem
            {
                Price = item.Price,
                CardEntry = new CardEntry(item.CardEntry.Card, item.CardEntry.Quantity)
            };
            _shopOffers.Add(newItem);
        }
        copy.Items = _shopOffers;
        copy.itemDeletionPrice = itemDeletionPrice;
        return copy;
    }
}
