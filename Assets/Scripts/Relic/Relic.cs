using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Relic", menuName = "Scriptable Objects/Relic")]
    public class Relic : ScriptableObject
    {
        public bool isInGame = true;
        [Space]
        public LocalizedString relicName;

        public Sprite icon;
        public Rarity rarity;
        public bool isNegative;
        public LocalizedString description;
        public LocalizedString shortDescription;

        [DeveloperNotes, SerializeField]
        private string _developerNotes;
    }
}
