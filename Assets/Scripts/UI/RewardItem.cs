using UnityEngine;
using UnityEngine.UI;
using static LootSet;

public class RewardItem : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _title;
    [SerializeField] private TMPro.TextMeshProUGUI _description;
    [SerializeField] private TMPro.TextMeshProUGUI _count;
    [SerializeField] private Image _icon;

    private LootSetData _lootItem;

    public RewardItem Setup(LootSetData lootSetData)
    {
        _lootItem = lootSetData;

        _title.text = _lootItem.Item.ItemName;
        _description.text = _lootItem.Item.Description;
        _count.text = _lootItem.Count.ToString();
        _icon.sprite = _lootItem.Item.Icon;

        return this;
    }
    public void OnRewardPicked()
    {
        if (_lootItem.Item == null)
        {
            Debug.LogError("LootItem is not set up");
            return;
        }

        _lootItem.Loot();
    }
}
