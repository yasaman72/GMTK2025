using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public class RarityInterface : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        public void SetVisuals(Rarity rarity)
        {
            RarityConfig rarityConfig = null;
            foreach (var config in GameDataBaseManager.GameDatabase.rarityConfigs)
            {
                if (config.rarity == rarity)
                {
                    rarityConfig = config;
                    break;
                }
            }

            if (rarityConfig == null)
            {
                Debug.LogWarning($"could not find the config for rarity {rarity}");
                return;
            }

            _image.color = rarityConfig.color;
            _text.text = rarityConfig.rarityName.GetLocalizedString();
        }
    }
}
