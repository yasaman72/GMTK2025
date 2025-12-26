using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LootSet;

namespace Deviloop
{
    public static class RewardManager
    {

        public static List<LootSetData> SelectRewards(List<LootSet> loots, int maxItems)
        {
            List<LootSetData> allRewards = loots.SelectMany(l => l.GetAllRewards()).ToList();

            LootSetData coinReward = allRewards.First(r => r.IsCoinLoot());

            // if more than one reward is coin, combine them into one
            if (allRewards.Count(r => r.IsCoinLoot()) > 1)
            {
                int totalCoins = allRewards.Sum(r => r.IsCoinLoot() ? r.Count : 0);
                CoinLoot coinItemCopy = allRewards.First(r => r.IsCoinLoot()).item as CoinLoot;
                coinReward = (
                    new LootSetData
                    {
                        item = coinItemCopy,
                        Count = totalCoins,
                    });
            }

            // only keep the non coin rewards
            allRewards = allRewards.Where(r => !(r.IsCoinLoot())).ToList();

            // TODO: better algorithm to pick the rewards, based on rarity, chance, seed and luck
            // right now the rarity of cards are taken into account in ItemLoot.ResetCard()
            ListUtilities.ShuffleItems(allRewards);
            allRewards = allRewards.GetRange(0, maxItems);
            allRewards.Add(coinReward);

            // reset card duplicates
            foreach (var reward in allRewards)
            {
                if (reward.item is ItemLoot item)
                {
                    int safety = 50;

                    while (safety-- > 0 &&
                           allRewards.Any(l =>
                               l != reward &&
                               l.item is ItemLoot itemLoot &&
                               itemLoot.Card == item.Card))
                    {
                        (reward.item as ItemLoot).ResetCard();
                    }

                    if (safety <= 0)
                    {
                        Debug.LogWarning("Failed to find unique card for loot item.");
                    }
                }
            }

            return allRewards;
        }
    }
}
