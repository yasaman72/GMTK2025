using Cards.ScriptableObjects;
using System;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    public static Action<CardDeck> OpenDeck;

    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private GameObject _deckViewItemPrefab;


    public void Initialize()
    {
        OpenDeck += onDeckOpen;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OpenDeck -= onDeckOpen;
    }

    private void onDeckOpen(CardDeck deck)
    {
        // TODO: pooling and not reseting everytime deck is opened
        foreach (Transform item in _deckContentHolder)
        {
            Destroy(item.gameObject);
        }

        foreach(var card in deck.GetAllCardsAsList())
        {
            var newCardPrefab = Instantiate(_deckViewItemPrefab, _deckContentHolder);
            newCardPrefab.GetComponent<DeckViewItem>().Setup(card);
        }
        gameObject.SetActive(true);
    }

    public void CloseDeck()
    {
        gameObject.SetActive(false);
    }
}
