using Cards;
using Cards.ScriptableObjects;
using Deviloop;
using TMPro;
using UnityEditor;
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
    [Header("Materials")]
    [SerializeField] private GameObject _materialIcon;
    [SerializeField] private Transform _materialsParent;

    public void Setup(BaseCard card)
    {
        _nameText.text = card.cardName.GetLocalizedString();
        _descriptionText.text = card.description.GetLocalizedString();
        _icon.sprite = card.cardIcon;

        foreach (Transform child in _materialsParent)
        {
            Destroy(child.gameObject);
        }

        // Setup material icon
        foreach (var material in GameDataBaseManager.GameDatabase.materials)
        {
            if (material.CompareMaterials(card.materialType))
            {
                GameObject materialObject = Instantiate(_materialIcon, _materialsParent);
                MaterialIcon materialIcon = materialObject.GetComponent<MaterialIcon>();
                materialIcon.Setup(material);
            }
        }
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
