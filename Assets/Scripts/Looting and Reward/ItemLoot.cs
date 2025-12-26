using Cards;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "ItemLoot", menuName = "Scriptable Objects/Loots/LootType/Item")]
    public class ItemLoot : LootItem
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
                    ResetCard();
                }
                return _card;
            }
        }

        public void ResetCard()
        {
            // TODO: write a utility safe while and do while loops
            int safety = 50;

            do
            {
                float totalChance = 0f;
                foreach (var card in GameDataBaseManager.GameDatabase.Cards)
                    totalChance += (int)card.cardRarity;

                float roll = Random.Range(0f, totalChance);
                float cumulative = 0f;

                foreach (var loot in GameDataBaseManager.GameDatabase.Cards)
                {
                    cumulative += (int)loot.cardRarity;
                    if (roll <= cumulative)
                    {
                        _card = loot;
                        break;
                    }
                }

                safety--;
                if (safety <= 0)
                {
                    Debug.LogWarning("Failed to find unique card for loot item.");
                    break;
                }
            } while (_card == null || _card.isNegativeItem || !_card.isInGame);
        }
    }
}
