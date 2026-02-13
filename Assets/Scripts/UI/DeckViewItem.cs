using Deviloop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckViewItem : MonoBehaviour, IPoolable
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

    public void OnSpawned()
    {
    }

    public void OnDespawned()
    {
        _button.onClick.RemoveAllListeners();
    }

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

        _priceText.text = card.price.ToString();
    }

    public void Setup(int price)
    {
        _priceText.text = price.ToString();
        Activate();
    }

    public void Deactivate()
    {
        _button.onClick.RemoveAllListeners();
        _descriptionText.text = "SOLD!";
    }

    public void Activate()
    {
        _button.interactable = true;
        _priceParent.SetActive(true);
        _descriptionText.text = "";
    }

    public void DisablePrice()
    {
        _priceParent.SetActive(false);
    }
}
