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
    [SerializeField] private Transform _itemsSelectionContent;
    [SerializeField] private GameObject _rewardItemPrefab;

    Dictionary<LootSetData, GameObject> _allCurrentLoots = new Dictionary<LootSetData, GameObject>();

    private List<GameObject> _rewardPrefabs = new List<GameObject>();
    public void Initialize()
    {
        OpenRewards += onDeckOpen;
        gameObject.SetActive(false);
        _allCurrentLoots.Clear();

        _rewardPrefabs.Clear();
        for (int i = 0; i < _deckContentHolder.transform.childCount; i++)
        {
            if (_deckContentHolder.transform.GetChild(i).GetComponent<RewardItem>() != null)
                _rewardPrefabs.Add(_deckContentHolder.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < _itemsSelectionContent.transform.childCount; i++)
        {
            if (_itemsSelectionContent.transform.GetChild(i).GetComponent<RewardItem>() != null)
                _rewardPrefabs.Add(_itemsSelectionContent.transform.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        OpenRewards -= onDeckOpen;
    }

    private void onDeckOpen(List<LootSet> loots)
    {
        Time.timeScale = 0;

        List<LootSetData> allRewards = RewardManager.SelectRewards(loots, _maxItemOption);

        for (int i = 0; i < allRewards.Count; i++)
        {
            GameObject newRewardPrefab = null;
            LootSetData reward = allRewards[i];

            if (_rewardPrefabs.Count > i)
            {
                if (reward.item is CardLoot && _rewardPrefabs[i].transform.parent.gameObject != _itemsSelectionContent)
                {
                    _rewardPrefabs[i].transform.SetParent(_itemsSelectionContent);
                    _itemsSelectionContent.gameObject.SetActive(true);
                }
                else
                    _rewardPrefabs[i].transform.SetParent(_deckContentHolder);
                newRewardPrefab = _rewardPrefabs[i];
                _rewardPrefabs[i].SetActive(true);
            }
            else
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
            var initialLoots = new Dictionary<LootSetData, GameObject>(_allCurrentLoots);
            foreach (var reward in initialLoots)
            {
                if (reward.Key.item is CardLoot)
                {
                    _allCurrentLoots.Remove(reward.Key);
                }
            }

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
        Time.timeScale = 1;
        gameObject.SetActive(false);
        _allCurrentLoots.Clear();
        OnRewardsClosed?.Invoke();
    }
}
