using Deviloop;
using Deviloop;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static Action OnShopClosedEvent;
    public static Action<ShopData> OpenShopAction;

    [SerializeField] private Transform _shopItemParent;
    [SerializeField] private GameObject _shopItemOption;
    [SerializeField] private GameObject _shopRemoveItemOption;
    [SerializeField] private GameObject _shoprerollOption;
    private List<GameObject> _currentShopCardButtons = new List<GameObject>();

    ShopData _shopData;

    private void Awake()
    {
        OpenShopAction += OnOpenShop;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OpenShopAction -= OnOpenShop;
    }

    private void OnOpenShop(ShopData shopData)
    {
        gameObject.SetActive(true);

        _shopData = shopData.SetupInstance();
        // TODO: pooling
        foreach (Transform child in _shopItemParent)
        {
            Destroy(child.gameObject);
        }

        CreateDeleteItemOption();
        CreateRerollOption();
        SetupCardOffers();
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
        OnShopClosedEvent?.Invoke();
    }


    private void SetupCardOffers()
    {
        _shopData = _shopData.SetupInstance();

        // TODO: pooling or reset
        foreach (GameObject child in _currentShopCardButtons)
        {
            Destroy(child);
        }

        foreach (var shopItem in _shopData.sellingCards)
        {
            CreateShopCardOption(shopItem);
        }
    }

    private void CreateShopCardOption(CardLoot cardLoot)
    {
        var shopItemButton = Instantiate(_shopItemOption, _shopItemParent);
        _currentShopCardButtons.Add(shopItemButton);
        shopItemButton.GetComponent<DeckViewItem>().Setup(cardLoot.Card);
        shopItemButton.GetComponent<Button>().onClick.AddListener(() => OnItemClick(cardLoot.Card, shopItemButton));
    }

    private void CreateDeleteItemOption()
    {
        var deleteItemButton = Instantiate(_shopRemoveItemOption, _shopItemParent);
        var deckViewItem = deleteItemButton.GetComponent<DeckViewItem>();
        deckViewItem.Setup(_shopData.itemDeletionPrice);
        deleteItemButton.GetComponent<Button>().onClick.AddListener(() => OnDeleteItemClick(deckViewItem));
    }

    private void CreateRerollOption()
    {
        var rerollButton = Instantiate(_shoprerollOption, _shopItemParent);
        rerollButton.GetComponent<DeckViewItem>().Setup(_shopData.rerollPrice);
        rerollButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (PlayerInventory.SpendCoin(_shopData.rerollPrice))
            {
                SetupCardOffers();
                AnalyticsManager.SendCustomEventAction?.Invoke("shop_rerolled", new Dictionary<string, object>());
            }
            else
            {
                Debug.Log("Not enough coins to reroll.");
            }
        });
    }

    private void OnItemClick(BaseCard card, GameObject itemButton)
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
            Debug.Log("Not enough coins to buy this item.");
        }
    }

    private void OnDeleteItemClick(DeckViewItem deleteItemButton)
    {
        Debug.Log("Openning item deletion window.");

        if (PlayerInventory.SpendCoin(_shopData.itemDeletionPrice))
        {
            DeckView.OpenDeckToDelete?.Invoke(CardManager.DrawDeck, 1);
            deleteItemButton.Deactivate();
        }
        else
        {
            Debug.Log("Not enough coins to delete item.");
        }
    }

    private void RemoveItemFromOffers(BaseCard card, GameObject itemButton)
    {
        var offer = _shopData.sellingCards.Find(c => c == card);

        Destroy(itemButton);
    }
}
