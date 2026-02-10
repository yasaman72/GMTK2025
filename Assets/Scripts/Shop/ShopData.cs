using Deviloop;
using Deviloop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Scriptable Objects/ShopData")]
public class ShopData : ScriptableObject
{
    [SerializeField] private int shopCardsCount = 3;
    [SerializeField] private List<BaseCard> shopPossibleCardOffers;
    [HideInInspector] public List<CardLoot> sellingCards;
    public int itemDeletionPrice = 20;
    public int rerollPrice = 10;

    public ShopData SetupInstance()
    {
        var copy = CreateInstance(nameof(ShopData)) as ShopData;
        copy.sellingCards = SetUniqueOffers();
        copy.itemDeletionPrice = itemDeletionPrice;
        copy.shopPossibleCardOffers = shopPossibleCardOffers;
        copy.rerollPrice = rerollPrice;
        copy.shopCardsCount = shopCardsCount;
        return copy;
    }

    private List<CardLoot> SetUniqueOffers()
    {
        var cardsList = new List<CardLoot>();
        for (int i = 0; i < shopCardsCount; i++)
        {
            CardLoot cardLoot = (CardLoot)CreateInstance(typeof(CardLoot));

            // TODO: turn into a utility function. update the reward manager as well
            int safety = 50;
            while (safety-- > 0 &&
                   cardsList.Any(card =>
                       cardLoot.IsSameLoot(card)))
            {
                cardLoot.ResetLoot(shopPossibleCardOffers);
            }

            cardsList.Add(cardLoot);
        }
        return cardsList;
    }

    [ContextMenu("Setup Instance")]
    private void TestSetupInstance()
    {
        sellingCards = SetUniqueOffers();
        Debug.Log($"Offered Cards Count: {sellingCards.Count}");
    }
}
