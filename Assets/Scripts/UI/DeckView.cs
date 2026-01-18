using Cards;
using Cards.ScriptableObjects;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckView : MonoBehaviour
{
    public static Action<CardDeck, Action<BaseCard>> OpenDeck;
    public static Action<CardDeck, int> OpenDeckToDelete;

    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private GameObject _deckViewItemPrefab;
    [SerializeField] private TextMeshProUGUI _deckTitle;

    private void Awake()
    {
        _deckTitle.text = "";
    }

    public void Initialize()
    {
        OpenDeck += onDeckOpen;
        OpenDeckToDelete += onOpenDeckToDelete;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OpenDeck -= onDeckOpen;
        OpenDeckToDelete -= onOpenDeckToDelete;
    }

    private void onDeckOpen(CardDeck deck, Action<BaseCard> OnCardClick = null)
    {
        // TODO: pooling and not reseting everytime deck is opened
        foreach (Transform item in _deckContentHolder)
        {
            Destroy(item.gameObject);
        }

        foreach (var card in deck.GetAllCardsAsList())
        {
            var newCardPrefab = Instantiate(_deckViewItemPrefab, _deckContentHolder);
            newCardPrefab.GetComponent<DeckViewItem>().Setup(card);
            if (OnCardClick != null)
            {
                newCardPrefab.GetComponent<Button>().onClick.AddListener(() => OnCardClick(card));
            }

        }
        gameObject.SetActive(true);
    }


    private void onOpenDeckToDelete(CardDeck deck, int cardsToDeleteCount = 1)
    {
        onDeckOpen(deck, (card) =>
        {
            CardManager.RemoveCardFromDeckAction?.Invoke(card, false);
            CloseDeck();
        });
        _deckTitle.text = "select an item to permanently delete from your deck";
    }

    public void CloseDeck()
    {
        _deckTitle.text = "";
        gameObject.SetActive(false);
    }
}
