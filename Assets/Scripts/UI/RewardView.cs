using Deviloop;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static LootSet;

public class RewardView : MonoBehaviour
{
    public static Action<List<LootSet>> OpenRewards;
    public static Action OnRewardsClosed;

    [SerializeField] private bool _shouldLog;
    [SerializeField] private int _maxLoot = 3;
    [Space]
    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private GameObject _rewardItemPrefab;

    Dictionary<LootSetData, GameObject> _allCurrentLoots = new Dictionary<LootSetData, GameObject>();

    public void Initialize()
    {
        OpenRewards += onDeckOpen;
        gameObject.SetActive(false);
        _allCurrentLoots.Clear();
    }

    private void OnDestroy()
    {
        OpenRewards -= onDeckOpen;
    }

    private void onDeckOpen(List<LootSet> loots)
    {
        Time.timeScale = 0;

        // TODO: pooling and not reseting everytime UI is opened
        foreach (Transform item in _deckContentHolder)
        {
            if (item)
                Destroy(item.gameObject);
        }

        List<LootSetData> allRewards = loots
            .SelectMany(l => l.GetPickedLoots())
            .ToList();
        ListUtilities.ShuffleItems(allRewards);

        // TODO: better algorithm to pick the rewards, based on rarity, seed and luck
        List<LootSetData> pickedRewards = allRewards.GetRange(0, Mathf.Min(_maxLoot, allRewards.Count));

        // if more than one reward is coin, combine them into one
        if (pickedRewards.Count(r => r.Item is CoinLoot) > 1)
        {
            int totalCoins = pickedRewards.Sum(r => r.Count);
            CoinLoot coinItemCopy = pickedRewards.First(r => r.Item is CoinLoot).Item as CoinLoot;
            // remove all coin rewards
            pickedRewards = pickedRewards.Where(r => !(r.Item is CoinLoot)).ToList();
            pickedRewards.Add(
                new LootSetData
                {
                    Item = coinItemCopy,
                    Count = totalCoins,
                    Chance = 1f
                });
        }


        foreach (var reward in pickedRewards)
        {
            // reset card duplicates
            if (reward.Item is ItemLoot item)
            {
                int safety = 50;

                while (safety-- > 0 &&
                       _allCurrentLoots.Any(l =>
                           l.Key.Item is ItemLoot itemLoot &&
                           itemLoot.Card == item.Card))
                {
                    (reward.Item as ItemLoot).ResetCard();
                }

                if (safety <= 0)
                {
                    Debug.LogWarning("Failed to find unique card for loot item.");
                }
            }

            LootSetData rewardCopy = reward.Clone();

            var newRewardPrefab = Instantiate(_rewardItemPrefab, _deckContentHolder);
            var rewardItem = newRewardPrefab.GetComponent<RewardItem>().Setup(rewardCopy);
            newRewardPrefab.GetComponent<Button>().onClick.AddListener(() => CollectReward(rewardCopy, newRewardPrefab));
            _allCurrentLoots.Add(rewardCopy, newRewardPrefab);
        }
        gameObject.SetActive(true);
    }

    public void CollectReward(LootSetData rewardItem, GameObject rewardPrefab)
    {
        rewardItem.Loot();
        _allCurrentLoots.Remove(rewardItem);

        if (rewardItem.Item is ItemLoot)
        {
            // remove all other item rewards from the option
            var initialLoots = new Dictionary<LootSetData, GameObject>(_allCurrentLoots);
            foreach (var reward in initialLoots)
            {
                if (reward.Key.Item is ItemLoot)
                {
                    _allCurrentLoots.Remove(reward.Key);
                    Destroy(reward.Value);
                }
            }
        }

        // TODO: use pooling
        Destroy(rewardPrefab);
    }

    public void CollectAllRewards()
    {
        foreach (var _lootItem in _allCurrentLoots)
        {
            if (_lootItem.Key.Item == null)
            {
                Debug.LogError("LootItem is not set up");
                return;
            }
            _lootItem.Key.Loot();
        }

        Close();
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        _allCurrentLoots.Clear();
        OnRewardsClosed?.Invoke();
    }
}
