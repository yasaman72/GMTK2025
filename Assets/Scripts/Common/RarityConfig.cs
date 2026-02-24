using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "NewRarityConfig", menuName = "Scriptable Objects/Configs/RarityConfig")]
    public class RarityConfig : ScriptableObject
    {
        public Rarity rarity;
        public LocalizedString rarityName;
        public Color color;
        public int basePrice;
    }
}
