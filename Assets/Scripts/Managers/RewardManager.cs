using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LootSet;

namespace Deviloop
{
    public static class RewardManager
    {
        public static List<LootSetData> SelectRewards(List<LootSet> loots, int maxCards = 2, int maxRelics = 1)
        {
            List<LootSetData> allRewards = loots.SelectMany(l => l.GetAllRewards()).ToList();

            LootSetData coinReward = allRewards.FirstOrDefault(r => r.IsCoinLoot());

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

            // add material loots
            List<LootSetData> materialsRewards = new List<LootSetData>();
            foreach (LootSetData reward in allRewards)
            {
                if (reward.item is MaterialLoot materialLoot)
                {
                    LootSetData result = materialsRewards.FirstOrDefault(m => m.item is MaterialLoot ml && ml.materialType == materialLoot.materialType);
                    if (result != null)
                    {
                        result.Count++;
                    }
                    else
                    {
                        materialsRewards.Add(reward);
                    }
                }
            }

            // remove material loots
            allRewards.RemoveAll(r => r.item is  MaterialLoot);

            // only keep the non coin rewards
            allRewards = allRewards.Where(r => !(r.IsCoinLoot())).ToList();


            // TODO: better algorithm to pick the rewards, based on rarity, chance, seed and luck
            // right now the rarity of cards are taken into account in ItemLoot.ResetCard()
            ListUtilities.ShuffleItems(allRewards);

            // only limit the number of card loots, not the relics
            // TODO: refactor
            allRewards = allRewards.Where(r => !(r.item is CardLoot)).ToList()
                .Concat(allRewards.Where(r => r.item is CardLoot).Take(maxCards))
                .ToList();

            allRewards = allRewards.Where(r => !(r.item is RelicLoot)).ToList()
                .Concat(allRewards.Where(r => r.item is RelicLoot).Take(maxRelics))
                .ToList();


            if (coinReward != null)
                allRewards.Add(coinReward);


            foreach (LootSetData materialReward in materialsRewards)
            {
                allRewards.Add(materialReward);
            }

            // reset card or relic duplicates
            foreach (var reward in allRewards)
            {
                if (reward.item is not MaterialLoot && reward.item is NonCoinLootItem item)
                {
                    int safety = 50;

                    while (safety-- > 0 &&
                           allRewards.Any(l =>
                               l != reward &&
                               l.item is NonCoinLootItem itemLoot &&
                               itemLoot.IsSameLoot(item)))
                    {
                        (reward.item as NonCoinLootItem).ResetLoot();
                    }

                    if (safety <= 0)
                    {
                        Debug.LogWarning("Failed to find unique non coin loots.");
                    }
                }
            }

            // if card or relic loots are null, remove them
            allRewards = allRewards.Where(r =>
                !(r.item is CardLoot cardLoot && cardLoot.Card == null) &&
                !(r.item is RelicLoot relicLoot && relicLoot.Relic == null)
            ).ToList();

            return allRewards;
        }
    }
}
