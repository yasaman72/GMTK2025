using Deviloop;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using static LootSet;

public class RewardView : MonoBehaviour
{
    public static Action<List<LootSet>> OpenRewards;
    public static Action OnRewardsClosed;

    [SerializeField] private bool _shouldLog;
    [SerializeField] private int _maxItemOption = 2;
    [SerializeField] private LocalizedString _notPickedRewardsMsg;
    [Space]
    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private Transform _itemsSelectionContent;
    [SerializeField] private GameObject _rewardItemPrefab;

    private bool _isRewardNotPickedUpNotificationDisplayed = false;

    Dictionary<LootSetData, GameObject> _allCurrentLoots = new Dictionary<LootSetData, GameObject>();

    private List<GameObject> _rewardPrefabs = new List<GameObject>();
    public void Initialize()
    {
        OpenRewards += onDeckOpen;
        gameObject.SetActive(false);
        _allCurrentLoots.Clear();
        CreateRewardPrefabs();
    }

    public void OnReset()
    {
        OpenRewards -= onDeckOpen;
    }

    private void CreateRewardPrefabs()
    {
        // TODO: write a more generic pool system for rewards and deck options
        _rewardPrefabs.Clear();

        foreach (Transform child in _deckContentHolder.transform)
        {
            if (child.GetComponent<RewardItem>() != null)
                _rewardPrefabs.Add(child.gameObject);
        }

        foreach (Transform child in _itemsSelectionContent.transform)
        {
            if (child.GetComponent<RewardItem>() != null && !_rewardPrefabs.Contains(child.gameObject))
                _rewardPrefabs.Add(child.gameObject);
        }
    }

    private void onDeckOpen(List<LootSet> loots)
    {
        Time.timeScale = 0;
        _allCurrentLoots.Clear();

        List<LootSetData> allRewards = RewardManager.SelectRewards(loots, _maxItemOption);

        for (int i = 0; i < allRewards.Count; i++)
        {
            GameObject newRewardPrefab = null;
            LootSetData reward = allRewards[i];

            if (_rewardPrefabs.Count <= i)
            {
                if (reward.item is CardLoot)
                {
                    newRewardPrefab = Instantiate(_rewardItemPrefab, _itemsSelectionContent);
                    _itemsSelectionContent.gameObject.SetActive(true);
                }
                else
                    newRewardPrefab = Instantiate(_rewardItemPrefab, _deckContentHolder);

                _rewardPrefabs.Add(newRewardPrefab);
            }

            if (reward.item is CardLoot && _rewardPrefabs[i].transform.parent.gameObject != _itemsSelectionContent)
            {
                _rewardPrefabs[i].transform.SetParent(_itemsSelectionContent);
                _itemsSelectionContent.gameObject.SetActive(true);
            }
            else
                _rewardPrefabs[i].transform.SetParent(_deckContentHolder);
            newRewardPrefab = _rewardPrefabs[i];
            _rewardPrefabs[i].SetActive(true);

            var rewardItem = newRewardPrefab.GetComponent<RewardItem>().Setup(reward);
            newRewardPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
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
        if (rewardItem.item is CardLoot)
        {
            _itemsSelectionContent.gameObject.SetActive(false);
        }

        rewardPrefab.gameObject.SetActive(false);
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
