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
    [Space]
    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private GameObject _rewardItemPrefab;

    List<LootSetData> _finalRewards = new List<LootSetData>();

    public void Initialize()
    {
        OpenRewards += onDeckOpen;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OpenRewards -= onDeckOpen;
    }

    private void onDeckOpen(List<LootSet> loot)
    {
        Time.timeScale = 0;

        // TODO: pooling and not reseting everytime UI is opened
        foreach (Transform item in _deckContentHolder)
        {
            if (item)
                Destroy(item.gameObject);
        }

        List<LootSetData> pickedRewards = new List<LootSetData>();

        foreach (var l in loot)
        {
            pickedRewards.AddRange(l.GetPickedLoots());
        }

        foreach (var reward in pickedRewards)
        {
            LootSetData rewardCopy = reward.Clone();
            rewardCopy.Setup();

            var newRewardPrefab = Instantiate(_rewardItemPrefab, _deckContentHolder);
            var rewardItem = newRewardPrefab.GetComponent<RewardItem>().Setup(rewardCopy);
            _finalRewards.Add(rewardCopy);
        }
        gameObject.SetActive(true);
    }

    public void CollectRewards()
    {
        foreach (var _lootItem in _finalRewards)
        {
            if (_lootItem.Item == null)
            {
                Debug.LogError("LootItem is not set up");
                return;
            }
            _lootItem.Loot();
        }

        Close();
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        OnRewardsClosed?.Invoke();
    }
}
