using System.Collections.Generic;
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
        [Space]
        public List<RelicEffectCompound> relicEffect;
        [Space]
        public Sprite icon;
        public Rarity rarity;
        public bool isNegative;
        public bool canHaveDuplicates = false;
        public LocalizedString description;
        public LocalizedString shortDescription;

        [DeveloperNotes, SerializeField]
        private string _developerNotes;

        [System.Serializable]
        public struct RelicEffectCompound
        {
            public BaseGameplayEvent gameplayEvent;
            public List<BaseRelicEffect> relicEffect;
        }
    }
}
