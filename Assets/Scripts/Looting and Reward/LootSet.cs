using Deviloop;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LootSet", menuName = "Scriptable Objects/Loots/LootSet", order = 1)]
public class LootSet : ScriptableObject
{
    [SerializeField] private List<LootSetData> _rewards;

    [System.Serializable]
    public class LootSetData
    {
        public LootItem item;
        [SerializeField] private int countMin, countMax;

        private int _count = 0;
        public int Count
        {
            set { _count = value; }
            get
            {
                if ((_count == 0))
                {
                    Count = Random.Range(countMin, countMax + 1);
                }
                return _count;
            }
        }

        public void Loot()
        {
            AudioManager.PlayAudioOneShot?.Invoke(item.OnLootSound);
            PlayerInventory.AddToInventoryAction?.Invoke(this);
        }

        public LootSetData Clone()
        {
            return new LootSetData
            {
                item = this.item,
                Count = this.Count,
                countMin = this.countMin,
                countMax = this.countMax
            };
        }

        public CoinLootResult IsCoinLoot()
        {
            if (item is CoinLoot coinLoot)
                return new CoinLootResult(coinLoot);

            return new CoinLootResult(null);
        }

        public class CoinLootResult
        {
            public CoinLoot Coin { get; }
            public bool IsCoin => Coin != null;

            public CoinLootResult(CoinLoot coin)
            {
                Coin = coin;
            }

            // Implicit conversion to bool
            public static implicit operator bool(CoinLootResult result)
            {
                return result != null && result.IsCoin;
            }
        }
    }

    public List<LootSetData> GetAllRewards()
    {
        foreach (var reward in _rewards)
        {
            reward.Count = 0;
        }

        List<LootSetData> rewards = new List<LootSetData>();

        foreach (var reward in _rewards)
        {
            if (reward.item is CoinLoot coinLoot)
            {
                // readd the coin, the count will be the number of coins rewarded
                var rewardsCopy = reward.Clone();
                rewards.Add(rewardsCopy);
            }
            else if (reward.item is ItemLoot itemLoot)
            {
                // add multiple card rewards
                for (int i = 0; i < reward.Count; i++)
                {
                    // create a copy of the LootSetData so we don't reuse the same object at runtime
                    var rewardsCopy = reward.Clone();
                    rewardsCopy.item = Object.Instantiate(itemLoot);
                    rewards.Add(rewardsCopy);
                }
            }
        }

        return rewards;
    }
}