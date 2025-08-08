using Cards.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckViewItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _nameText;

    public void Setup(BaseCard card)
    {
        _nameText.text = card.cardName;
        _nameText.text = card.description;
        _icon.sprite = card.cardIcon;
    }
}
