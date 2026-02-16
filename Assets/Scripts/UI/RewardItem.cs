using Deviloop;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using static LootSet;

public class RewardItem : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private Color _itemRewardsBackground = Color.white;

    private TooltipTrigger _tooltipTrigger;

    public void OnDespawned()
    {

    }

    public void OnSpawned()
    {
    }

    public RewardItem Setup(LootSetData lootSetData)
    {
        if (lootSetData.item is CardLoot cardLoot)
        {
            var card = cardLoot.Card;
            _title.text = card.cardName.GetLocalizedString();
            _description.text = card.description.GetLocalizedString();
            _count.text = "";
            _icon.sprite = card.cardIcon;
            _background.color = _itemRewardsBackground;
        }
        else if (lootSetData.item is RelicLoot relicLoot && relicLoot.Relic != null)
        {
            var relic = relicLoot.Relic;
            _title.text = relic.relicName.GetLocalizedString();
            _description.text = relic.shortDescription.GetLocalizedString();
            _count.text = "";
            _icon.sprite = relic.icon;
            _background.color = _itemRewardsBackground;
            _tooltipTrigger = _background.gameObject.AddComponent<TooltipTrigger>();
            SetTooltipText(relic.description);
        }
        else
        {
            _title.text = lootSetData.item.itemName;
            _description.text = lootSetData.item.description;
            _count.text = lootSetData.Count.ToString();
            _icon.sprite = lootSetData.item.icon;
        }
        return this;
    }

    private void SetTooltipText(LocalizedString localizedString)
    {
        if (_tooltipTrigger == null)
            _tooltipTrigger = GetComponent<TooltipTrigger>();

        _tooltipTrigger.SetLocalizedString(localizedString);
    }
}
