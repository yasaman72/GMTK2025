using Codice.CM.Common;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "RelicLoot", menuName = "Scriptable Objects/Loots/LootType/Relic")]

    public class RelicLoot : NonCoinLootItem
    {
        private Relic _relic = null;

        public Relic Relic
        {
            get
            {
                if (_relic == null)
                {
                    ResetLoot();
                }
                return _relic;
            }
        }

        public override bool IsSameLoot(NonCoinLootItem other)
        {
            if (other is RelicLoot otherRelicLoot)
            {
                return this.Relic == otherRelicLoot.Relic;
            }
            return false;
        }

        public override void ResetLoot()
        {
            // TODO: since relic and card loot share similar logic, refactor to another class or utility
            // TODO: write a utility safe while and do while loops
            int safety = 50;

            do
            {
                var allRelics = GameDataBaseManager.GameDatabase.relics;
                var playerRelics = RelicManager.OwnedRelics;

                // filter out owned relics
                allRelics = allRelics.Where(r => !playerRelics.Contains(r)).ToList();

                if (allRelics.Count <= 0)
                {
                    Debug.LogWarning("No available relics to select from for loot item.");
                    _relic = null;
                    break;
                }

                float totalChance = 0f;
                foreach (var relic in allRelics)
                    totalChance += (int)relic.rarity;

                float roll = Random.Range(0f, totalChance);
                float cumulative = 0f;

                foreach (var relic in allRelics)
                {
                    cumulative += (int)relic.rarity;
                    if (roll <= cumulative)
                    {
                        _relic = relic;
                        break;
                    }
                }

                safety--;
                if (safety <= 0)
                {
                    Debug.LogWarning("Failed to find unique relic for loot item.");
                    break;
                }
            } while (_relic == null || _relic.isNegative || !_relic.isInGame);
        }
    }
}
