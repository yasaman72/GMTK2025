using Cards;
using Cards.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopData;

public class DeckViewItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _countText;
    [Header("Shop")]
    [SerializeField] private GameObject _priceParent;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _button;


    public void Setup(BaseCard card)
    {
        _nameText.text = card.cardName;
        _descriptionText.text = card.description;
        _icon.sprite = card.cardIcon;
    }

    public void Setup(CardEntry card)
    {
        Setup(card.Card);
        _countText.text = card.Quantity.ToString() + "X";
    }

    public void Setup(ShopItem shopItem)
    {
        Setup(shopItem.CardEntry);
        _priceParent.SetActive(true);
        _priceText.text = shopItem.Price.ToString();
    }

    public void Setup(int price)
    {
        _priceText.text = price.ToString();
    }

    public void Deactivate()
    {
        _button.onClick.RemoveAllListeners();
        _descriptionText.text = "SOLD!";
    }
}
