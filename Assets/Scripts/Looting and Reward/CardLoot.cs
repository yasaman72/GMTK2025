using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "CardLoot", menuName = "Scriptable Objects/Loots/LootType/Card")]
    public class CardLoot : NonCoinLootItem
    {
        // TODO: consider the rarity of cards when generating loot
        // TODO: randomly pick a card based on rarity instead of assigning a specific card
        private BaseCard _card = null;

        public BaseCard Card
        {
            get
            {
                if (_card == null)
                {
                    ResetLoot();
                }
                return _card;
            }
        }

        public override bool IsSameLoot(NonCoinLootItem other)
        {
            if (other is CardLoot otherItemLoot)
            {
                return this.Card == otherItemLoot.Card;
            }
            return false;
        }

        public CardLoot ResetLoot(List<BaseCard> allCards)
        {
            PickRandomCard(allCards);
            return this;
        }

        public override void ResetLoot()
        {
            PickRandomCard(GameDataBaseManager.GameDatabase.cards);
        }

        private void PickRandomCard(List<BaseCard> allCards)
        {
            // TODO: write a utility safe while and do while loops
            int safety = 50;

            do
            {
                int totalChance = 0;
                foreach (var card in allCards)
                    totalChance += (int)card.rarity;

                int roll = SeededRandom.Range(0, totalChance);
                int cumulative = 0;

                foreach (var loot in allCards)
                {
                    cumulative += (int)loot.rarity;
                    if (roll <= cumulative)
                    {
                        _card = loot;
                        break;
                    }
                }

                safety--;
                if (safety <= 0)
                {
                    Debug.LogWarning("Failed to find unique card for loot card.");
                    break;
                }
            } while (_card == null || _card.isNegative || !_card.isInGame);
        }
    }
}
