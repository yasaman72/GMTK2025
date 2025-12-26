using Deviloop;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LootSet;

public class RewardView : MonoBehaviour
{
    public static Action<List<LootSet>> OpenRewards;
    public static Action OnRewardsClosed;

    [SerializeField] private bool _shouldLog;
    [SerializeField] private int _maxItemOption = 2;
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

        List<LootSetData> allRewards = RewardManager.SelectRewards(loots, _maxItemOption);

        foreach (var reward in allRewards)
        {
            var newRewardPrefab = Instantiate(_rewardItemPrefab, _deckContentHolder);
            var rewardItem = newRewardPrefab.GetComponent<RewardItem>().Setup(reward);
            newRewardPrefab.GetComponent<Button>().onClick.AddListener(() => CollectReward(reward, newRewardPrefab));
            _allCurrentLoots.Add(reward, newRewardPrefab);
        }
        gameObject.SetActive(true);
    }

    public void CollectReward(LootSetData rewardItem, GameObject rewardPrefab)
    {
        rewardItem.Loot();
        _allCurrentLoots.Remove(rewardItem);

        // remove all other item rewards from the option
        if (rewardItem.item is ItemLoot)
        {
            var initialLoots = new Dictionary<LootSetData, GameObject>(_allCurrentLoots);
            foreach (var reward in initialLoots)
            {
                if (reward.Key.item is ItemLoot)
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
            if (_lootItem.Key.item == null)
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
