using Cards;
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
    [SerializeField] private GameObject _shopItem;

    List<ShopItem> _shopOffers;

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

        _shopOffers = shopData.Copy();
        // TODO: pooling
        foreach (Transform child in _shopItemParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var shopItem in _shopOffers)
        {
            CreateShopItem(shopItem);
        }
    }
    public void CloseShop()
    {
        gameObject.SetActive(false);
        OnShopClosedEvent?.Invoke();
    }

    private void CreateShopItem(ShopItem shopItemData)
    {
        var shopItemButton = Instantiate(_shopItem, _shopItemParent);
        shopItemButton.GetComponent<DeckViewItem>().Setup(shopItemData);
        shopItemButton.GetComponent<Button>().onClick.AddListener(() => OnItemClick(shopItemData, shopItemButton));
    }

    private void OnItemClick(ShopItem shopItemData, GameObject itemButton)
    {
        if (PlayerInventory.SpendCoin(shopItemData.Price))
        {
            CardManager.AddCardToDeckAction?.Invoke(shopItemData.CardEntry);
            RemoveItemFromOffers(shopItemData, itemButton);
        }
        else
        {
            Debug.Log("Not enough coins to buy this card.");
        }
    }

    private void RemoveItemFromOffers(ShopItem shopItem, GameObject itemButton)
    {
        var offer = _shopOffers.Find(o => o.CardEntry.Card == shopItem.CardEntry.Card);

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
