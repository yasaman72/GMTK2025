using Cards;
using Cards.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DeckViewItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _countText;

    public void Setup(BaseCard card)
    {
        _nameText.text = card.cardName;
        _descriptionText.text = card.description;
        _icon.sprite = card.cardIcon;
    }

    public void Setup(CardEntry card)
    {
        Setup(card.cardType);
        _countText.text = card.quantity.ToString() + "X";
    }
}
