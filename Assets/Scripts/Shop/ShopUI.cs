using Cards;
using Deviloop;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ShopData;

public class ShopUI : MonoBehaviour
{
    public static Action OnShopClosedEvent;
    public static Action<ShopData> OpenShopAction;

    [SerializeField] private Transform _shopItemParent;
    [SerializeField] private GameObject _shopItemOption;
    [SerializeField] private GameObject _shopRemoveItemOption;

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

        _shopData = shopData.Copy();
        // TODO: pooling
        foreach (Transform child in _shopItemParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var shopItem in _shopData.Items)
        {
            CreateShopItemOption(shopItem);
        }
        CreateDeleteItemOption();
    }
    public void CloseShop()
    {
        gameObject.SetActive(false);
        OnShopClosedEvent?.Invoke();
    }

    private void CreateShopItemOption(ShopItem shopItemData)
    {
        var shopItemButton = Instantiate(_shopItemOption, _shopItemParent);
        shopItemButton.GetComponent<DeckViewItem>().Setup(shopItemData);
        shopItemButton.GetComponent<Button>().onClick.AddListener(() => OnItemClick(shopItemData, shopItemButton));
    }

    private void CreateDeleteItemOption()
    {
        var deleteItemButton = Instantiate(_shopRemoveItemOption, _shopItemParent);
        var deckViewItem = deleteItemButton.GetComponent<DeckViewItem>();
        deckViewItem.Setup(_shopData.itemDeletionPrice);
        deleteItemButton.GetComponent<Button>().onClick.AddListener(() => OnDeleteItemClick(deckViewItem));
    }

    private void OnItemClick(ShopItem shopItemData, GameObject itemButton)
    {
        if (PlayerInventory.SpendCoin(shopItemData.Price))
        {
            CardManager.AddCardToDeckAction?.Invoke(shopItemData.CardEntry);
            RemoveItemFromOffers(shopItemData, itemButton);
            AnalyticsManager.SendCustomEventAction?.Invoke("shop_item_bought", new Dictionary<string, object>
            {
                { "item_name", shopItemData.CardEntry.Card.cardName}
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

    private void RemoveItemFromOffers(ShopItem shopItem, GameObject itemButton)
    {
        var offer = _shopData.Items.Find(o => o.CardEntry.Card == shopItem.CardEntry.Card);

        offer.CardEntry.Quantity -= shopItem.CardEntry.Quantity;

        if (offer.CardEntry.Quantity > 0)
        {
            itemButton.GetComponent<DeckViewItem>().Setup(shopItem);
        }
        else
        {
            Destroy(itemButton);
        }
    }
}
