using System;
using UnityEngine;
using UnityEngine.UI;
using static LootSet;

public class RewardView : MonoBehaviour
{
    public static Action<LootSet> OpenRewards;
    public static Action OnRewardsClosed;

    [SerializeField] private bool _shouldLog;
    [Space]
    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private GameObject _rewardItemPrefab;


    public void Initialize()
    {
        OpenRewards += onDeckOpen;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OpenRewards -= onDeckOpen;
    }

    private void onDeckOpen(LootSet loot)
    {
        Time.timeScale = 0;

        // TODO: pooling and not reseting everytime deck is opened
        foreach (Transform item in _deckContentHolder)
        {
            if (item)
                Destroy(item.gameObject);
        }

        var pickedRewards = loot.GetPickedLoots();
        foreach (var reward in pickedRewards)
        {
            reward.Reset();

            var newRewardPrefab = Instantiate(_rewardItemPrefab, _deckContentHolder);
            var rewardItem = newRewardPrefab.GetComponent<RewardItem>().Setup(reward);
            newRewardPrefab.AddComponent<Button>().onClick.AddListener(() =>
            {
                rewardItem.OnRewardPicked();
                Close();
            });
        }
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        OnRewardsClosed?.Invoke();
    }
}
