using Deviloop;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIView, IInitiatable
{
    public static Action OnShopClosedEvent;
    public static Action<ShopData> OpenShopAction;

    [SerializeField] private bool _shouldLog = false;
    [Space]
    [SerializeField] private Transform _shopItemParent;
    [SerializeField] private DeckViewItem _shopItemOption;
    [SerializeField] private GameObject _shopRemoveItemOption;
    [SerializeField] private GameObject _shoprerollOption;
    private List<DeckViewItem> _currentShopCardButtons = new List<DeckViewItem>();

    private ShopData _shopData;
    private ObjectPool<DeckViewItem> _itemsPool;
    private GameObject _deleteItemButton;
    private GameObject _rerollButton;


    public override void Initiate()
    {
        OpenShopAction += OnOpenShop;
        gameObject.SetActive(false);

        foreach (Transform child in _shopItemParent)
        {
            Destroy(child.gameObject);
        }

        try
        {
            _itemsPool = PoolManager.Instance.GetPool(_shopItemOption);
        }
        catch
        {
            _itemsPool = PoolManager.Instance.CreatePool(_shopItemOption, 5);
        }
    }

    private void OnDestroy()
    {
        OpenShopAction -= OnOpenShop;
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        ResetOffers();
        _currentShopCardButtons.Clear();
        gameObject.SetActive(false);
        OnShopClosedEvent?.Invoke();

        UIViewsManager.Instance.ClosePage();
    }


    private void OnOpenShop(ShopData shopData)
    {
        UIViewsManager.Instance.OpenPage(this);
        _shopData = shopData.SetupInstance();

        CreateDeleteItemOption();
        CreateRerollOption();

        SetupCardOffers();
    }

    private void SetupCardOffers()
    {
        _shopData = _shopData.SetupInstance();
        ResetOffers();

        foreach (var shopItem in _shopData.sellingCards)
        {
            CreateShopCardOption(shopItem);
        }
    }

    private void ResetOffers()
    {
        foreach (DeckViewItem child in _currentShopCardButtons)
        {
            if (child.isActiveAndEnabled)
                _itemsPool.ReturnToPool(child);
        }
    }

    private void CreateShopCardOption(CardLoot cardLoot)
    {
        var shopItemButton = _itemsPool.Get();
        shopItemButton.transform.SetParent(_shopItemParent, false);
        shopItemButton.transform.localScale = Vector2.one;
        _currentShopCardButtons.Add(shopItemButton);
        shopItemButton.GetComponent<DeckViewItem>().Setup(cardLoot.Card);
        shopItemButton.GetComponent<Button>().onClick.AddListener(() => OnItemClick(cardLoot.Card, shopItemButton));
    }

    private void CreateDeleteItemOption()
    {
        if (_deleteItemButton == null)
            _deleteItemButton = Instantiate(_shopRemoveItemOption, _shopItemParent);

        DeckViewItem deckViewItem = _deleteItemButton.GetComponent<DeckViewItem>();
        deckViewItem.Setup(_shopData.itemDeletionPrice);
        _deleteItemButton.GetComponent<Button>().onClick.AddListener(() => OnDeleteItemClick(deckViewItem));
    }

    private void CreateRerollOption()
    {
        if (_rerollButton == null)
            _rerollButton = Instantiate(_shoprerollOption, _shopItemParent);

        _rerollButton.GetComponent<DeckViewItem>().Setup(_shopData.rerollPrice);
        _rerollButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _rerollButton.GetComponent<Button>().onClick.AddListener(() => OnReroll());
    }

    private void OnReroll()
    {
        if (PlayerInventory.SpendCoin(_shopData.rerollPrice))
        {
            SetupCardOffers();
            AnalyticsManager.SendCustomEventAction?.Invoke("shop_rerolled", new Dictionary<string, object>());
        }
        else
        {
            Logger.Log("Not enough coins to reroll.", _shouldLog);
        }
    }

    private void OnItemClick(BaseCard card, DeckViewItem itemButton)
    {
        if (PlayerInventory.SpendCoin(card.price))
        {
            CardManager.AddCardToDeckAction?.Invoke(card, 1);
            RemoveItemFromOffers(card, itemButton);
            AnalyticsManager.SendCustomEventAction?.Invoke("shop_item_bought", new Dictionary<string, object>
            {
                { "item_name", card.cardName}
            });
        }
        else
        {
            Logger.Log("Not enough coins to buy this item.", _shouldLog);
        }
    }

    private void OnDeleteItemClick(DeckViewItem deleteItemButton)
    {
        Logger.Log("Openning item deletion window.", _shouldLog);

        if (PlayerInventory.SpendCoin(_shopData.itemDeletionPrice))
        {
            DeckView.OpenDeckToDelete?.Invoke(CardManager.DrawDeck, 1, () => OnDeleteAnItem(deleteItemButton));
        }
        else
        {
            Logger.Log("Not enough coins to delete item.", _shouldLog);
        }
    }

    public void OnDeleteAnItem(DeckViewItem deleteItemButton)
    {
        deleteItemButton.Deactivate();
    }

    private void RemoveItemFromOffers(BaseCard card, DeckViewItem itemButton)
    {
        var offer = _shopData.sellingCards.Find(c => c == card);
        _itemsPool.ReturnToPool(itemButton);
    }
}
