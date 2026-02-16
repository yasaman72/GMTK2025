using Deviloop;
using Deviloop.ScriptableObjects;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckView : MonoBehaviour
{
    public delegate void OpenDeckDelegate(CardDeck deck, Action<BaseCard> OnCardClick = null, bool showPrice = false);
    public static OpenDeckDelegate OpenDeck;
    public static Action<CardDeck, int, Action> OpenDeckToDelete;

    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Transform _deckContentHolder;
    [SerializeField] private DeckViewItem _cardPrefab;
    [SerializeField] private TextMeshProUGUI _deckTitle;

    private ObjectPool<DeckViewItem> _itemsPool;
    private List<DeckViewItem> _activeItems = new List<DeckViewItem>();


    public void Initialize()
    {
        OpenDeck += onDeckOpen;
        OpenDeckToDelete += onOpenDeckToDelete;
        gameObject.SetActive(false);
    }

    public void OnReset()
    {
        OpenDeck -= onDeckOpen;
        OpenDeckToDelete -= onOpenDeckToDelete;
    }

    private void onDeckOpen(CardDeck deck, Action<BaseCard> OnCardClick = null, bool showPrice = true)
    {
        _deckTitle.text = "";

        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 1f;
        _scrollRect.horizontalNormalizedPosition = 0f;

        try
        {
            _itemsPool = PoolManager.Instance.GetPool(_cardPrefab);
        }
        catch (Exception)
        {
            _itemsPool = PoolManager.Instance.CreatePool(_cardPrefab, 20);
        }

        foreach (var card in deck.GetAllCardsAsList())
        {
            var newCardPrefab = _itemsPool.Get();
            newCardPrefab.transform.SetParent(_deckContentHolder);
            newCardPrefab.transform.localScale = Vector3.one;
            var deckViewItem = newCardPrefab.GetComponent<DeckViewItem>();
            _activeItems.Add(deckViewItem);
            deckViewItem.Setup(card);
            if (OnCardClick != null)
            {
                newCardPrefab.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnCardClick(card));
            }

            if (!showPrice)
            {
                deckViewItem.DisablePrice();
            }

        }
        gameObject.SetActive(true);
    }


    private void onOpenDeckToDelete(CardDeck deck, int cardsToDeleteCount = 1, Action callback = null)
    {
        onDeckOpen(deck, (card) =>
        {
            CardManager.RemoveCardFromDeckAction?.Invoke(card, false);
            CloseDeck();
            callback?.Invoke();
        }, false);
        _deckTitle.text = "select an item to permanently delete from your deck";
    }

    public void CloseDeck()
    {
        _deckTitle.text = "";
        gameObject.SetActive(false);

        foreach (var item in _activeItems)
            _itemsPool.ReturnToPool(item);

        _activeItems.Clear();
    }
}
