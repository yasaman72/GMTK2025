using Deviloop;
using UnityEngine;
using UnityEngine.UI;
using static LootSet;

public class RewardItem : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _title;
    [SerializeField] private TMPro.TextMeshProUGUI _description;
    [SerializeField] private TMPro.TextMeshProUGUI _count;
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private Color _itemRewardsBackground = Color.white;


    public RewardItem Setup(LootSetData lootSetData)
    {
        if (lootSetData.Item is ItemLoot item)
        {
            var card = item.Card;
            _title.text = card.cardName;
            _description.text = card.description;
            _count.text = "";
            _icon.sprite = card.cardIcon;
            _background.color = _itemRewardsBackground;
        }
        else
        {
            _title.text = lootSetData.Item.itemName;
            _description.text = lootSetData.Item.description;
            _count.text = lootSetData.Count.ToString();
            _icon.sprite = lootSetData.Item.icon;
        }

        return this;
    }
}
