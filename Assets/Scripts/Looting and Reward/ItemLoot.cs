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
                    _card = ListUtilities.GetRandomElement<BaseCard>(GameDataBaseManager.GameDatabase.Cards.ToList());
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
                _card = ListUtilities.GetRandomElement<BaseCard>(GameDataBaseManager.GameDatabase.Cards.ToList());
                safety--;
                if (safety <= 0)
                {
                    Debug.LogWarning("Failed to find unique card for loot item.");
                    break;
                }
            } while (_card == null || _card.isNegativeItem);
        }
    }
}
