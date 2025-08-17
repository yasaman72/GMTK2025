using Cards;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData")]
public class ShopData : ScriptableObject
{
    public List<ShopItem> Items;

    [System.Serializable]
    public struct ShopItem
    {
        public CardEntry CardEntry;
        public int Price;
    }

    public List<ShopItem> Copy()
    {
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
        return _shopOffers;
    }
}
