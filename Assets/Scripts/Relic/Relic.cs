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
        public List<RelicEffectCompound> relicEffectCompound;
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
            [SerializeReference, SubclassSelector]
            public BaseGameplayEvent gameplayEvent;

            [SerializeReference, SubclassSelector]
            public List<BasePredicate> predicates;

            [SerializeReference, SubclassSelector]
            public List<BaseRelicEffect> relicEffect;
        }
    }
}
