using Cards;
using Cards.ScriptableObjects;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : MonoBehaviour
{
    public static Action<CardDeck> OpenRewards;

    [SerializeField] private bool _shouldLog;
    [Space]
    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private GameObject _deckViewItemPrefab;
    [SerializeField] private CardManager _cardManager;


    public void Initialize()
    {
        OpenRewards += onDeckOpen;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OpenRewards -= onDeckOpen;
    }

    private void onDeckOpen(CardDeck deck)
    {
        Time.timeScale = 0;

        // TODO: pooling and not reseting everytime deck is opened
        foreach (Transform item in _deckContentHolder)
        {
            if (item)
                Destroy(item.gameObject);
        }

        // TODO: update "CardDeck" to handle runtime deck and data file decks
        foreach (var card in deck.startingCards)
        {
            var newCardPrefab = Instantiate(_deckViewItemPrefab, _deckContentHolder);
            newCardPrefab.GetComponent<DeckViewItem>().Setup(card);
            newCardPrefab.AddComponent<Button>().onClick.AddListener(() => OnRewardPicked(card));
        }
        gameObject.SetActive(true);
    }

    public void OnRewardPicked(CardEntry card)
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);

        Logger.Log("add card " + card.cardType.cardName, _shouldLog);

        _cardManager.AddCardToDiscard(card);
    }
}
