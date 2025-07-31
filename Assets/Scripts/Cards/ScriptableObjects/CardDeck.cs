using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardDeck", menuName = "Cards/Card Deck")]
    public class CardDeck : ScriptableObject
    {
        [Header("Deck Configuration")]
        public List<CardEntry> startingCards = new List<CardEntry>();
    
        [Header("Deck Info")]
        [SerializeField] private int totalCards;
    
        // Runtime deck state (this will be copied to CardManager for actual gameplay)
        [System.NonSerialized]
        public Dictionary<BaseCard, int> currentDeck = new Dictionary<BaseCard, int>();
    
        private void OnValidate()
        {
            // Update total cards count in inspector
            totalCards = 0;
            foreach (var entry in startingCards)
            {
                if (entry.cardType != null)
                    totalCards += entry.quantity;
            }
        }
    
        public void InitializeDeck()
        {
            currentDeck.Clear();
            foreach (var entry in startingCards)
            {
                if (entry.cardType != null && entry.quantity > 0)
                {
                    currentDeck[entry.cardType] = entry.quantity;
                }
            }
        }
    
        public int GetTotalCardCount()
        {
            int total = 0;
            foreach (var kvp in currentDeck)
            {
                total += kvp.Value;
            }
            return total;
        }
    
        public List<BaseCard> GetAllCardsAsList()
        {
            List<BaseCard> allCards = new List<BaseCard>();
            foreach (var kvp in currentDeck)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    allCards.Add(kvp.Key);
                }
            }
            return allCards;
        }
    
        public bool RemoveCard(BaseCard card)
        {
            if (currentDeck.ContainsKey(card) && currentDeck[card] > 0)
            {
                currentDeck[card]--;
                if (currentDeck[card] <= 0)
                {
                    currentDeck.Remove(card);
                }
                return true;
            }
            return false;
        }
    
        public void AddCard(BaseCard card, int quantity = 1)
        {
            if (currentDeck.ContainsKey(card))
            {
                currentDeck[card] += quantity;
            }
            else
            {
                currentDeck[card] = quantity;
            }
        }
    }
}