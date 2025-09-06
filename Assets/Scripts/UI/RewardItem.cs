using UnityEngine;
using UnityEngine.UI;
using static LootSet;

public class RewardItem : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _title;
    [SerializeField] private TMPro.TextMeshProUGUI _description;
    [SerializeField] private TMPro.TextMeshProUGUI _count;
    [SerializeField] private Image _icon;


    public RewardItem Setup(LootSetData lootSetData)
    {
        _title.text = lootSetData.Item.ItemName;
        _description.text = lootSetData.Item.Description;
        _count.text = lootSetData.Count.ToString();
        _icon.sprite = lootSetData.Item.Icon;

        return this;
    }
}
