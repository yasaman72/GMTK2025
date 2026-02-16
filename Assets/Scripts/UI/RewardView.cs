using Deviloop;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using static LootSet;

public class RewardView : MonoBehaviour, IInitiatable
{
    public static Action<List<LootSet>> OpenRewards;
    public static Action OnRewardsClosed;

    [SerializeField] private bool _shouldLog;
    [SerializeField] private int _maxItemOption = 2;
    [SerializeField] private LocalizedString _notPickedRewardsMsg;
    [Space]
    [SerializeField] private ModifiableFloat _waitBeforeShowingRewards = new ModifiableFloat(3);
    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private Transform _itemsSelectionContent;
    [SerializeField] private RewardItem _rewardItemPrefab;

    private bool _isRewardNotPickedUpNotificationDisplayed = false;

    Dictionary<LootSetData, RewardItem> _allCurrentLoots = new Dictionary<LootSetData, RewardItem>();

    private ObjectPool<RewardItem> _rewardItemsPool;
    private List<RewardItem> _currentCardItems = new List<RewardItem>();

    public void Initiate()
    {
        OpenRewards += onDeckOpen;
        gameObject.SetActive(false);
        _allCurrentLoots.Clear();

        _rewardItemsPool = PoolManager.Instance.CreatePool(_rewardItemPrefab, 3);
    }

    public void OnReset()
    {
        OpenRewards -= onDeckOpen;
    }

    private async void onDeckOpen(List<LootSet> loots)
    {
        await Awaitable.WaitForSecondsAsync(_waitBeforeShowingRewards.Value);

        Time.timeScale = 0;
        _allCurrentLoots.Clear();

        List<LootSetData> allRewards = RewardManager.SelectRewards(loots, _maxItemOption);

        for (int i = 0; i < allRewards.Count; i++)
        {
            RewardItem rewardItem = _rewardItemsPool.Get();
            LootSetData reward = allRewards[i];

            if (reward.item is CardLoot)
            {
                rewardItem.transform.SetParent(_itemsSelectionContent);
                rewardItem.gameObject.SetActive(true);
                _itemsSelectionContent.gameObject.SetActive(true);
                _currentCardItems.Add(rewardItem);
            }
            else
            {
                rewardItem.transform.SetParent(_deckContentHolder);
                rewardItem.gameObject.SetActive(false);
            }
            rewardItem.gameObject.SetActive(true);
            rewardItem.transform.localScale = Vector3.one;

            rewardItem.Setup(reward);
            rewardItem.GetComponent<Button>().onClick.RemoveAllListeners();
            rewardItem.GetComponent<Button>().onClick.AddListener(() => CollectReward(reward, rewardItem));
            _allCurrentLoots.Add(reward, rewardItem);
        }

        gameObject.SetActive(true);
    }

    public void CollectReward(LootSetData rewardItem, RewardItem rewardPrefab)
    {
        rewardItem.Loot();
        _allCurrentLoots.Remove(rewardItem);

        // remove all other item rewards from the option
        if (rewardItem.item is CardLoot)
        {
            foreach (var reward in _currentCardItems)
            {
                _rewardItemsPool.ReturnToPool(reward);
            }
        }

        _rewardItemsPool.ReturnToPool(rewardPrefab);
        _itemsSelectionContent.gameObject.SetActive(false);
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
        if (!_isRewardNotPickedUpNotificationDisplayed && !CheckIfPickedAllRewards())
        {
            _isRewardNotPickedUpNotificationDisplayed = true;
            MessageController.OnDisplayMessage?.Invoke("You haven't picked all your rewards, you sure you want to continue?", 2);
            return;
        }

        _isRewardNotPickedUpNotificationDisplayed = false;
        Time.timeScale = 1;
        gameObject.SetActive(false);
        _allCurrentLoots.Clear();
        OnRewardsClosed?.Invoke();
        _currentCardItems.Clear();
    }


    private bool CheckIfPickedAllRewards()
    {
        var allRewardItems = _deckContentHolder.GetComponentsInChildren<RewardItem>();
        if (allRewardItems.Length == 0)
        {
            return true;
        }
        return false;
    }
}
