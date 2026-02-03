using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Relic", menuName = "Scriptable Objects/Relic")]
    public class Relic : GUIDScriptableObject
    {
        public Action AfterApply;

        public bool isInGame = true;
        [Space]
        public LocalizedString relicName;
        [Space]
        public RelicEffectCompound relicEffectCompound;
        [Space, SpritePreview(64)]
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
            public bool isPassive;
            [SerializeReference, SubclassSelector]
            public BaseGameplayEvent gameplayEvent;

            [SerializeReference, SubclassSelector]
            public List<BasePredicate> predicates;

            [SerializeReference, SubclassSelector]
            public List<BaseRelicEffect> relicEffect;
        }

#if UNITY_EDITOR
        protected new void OnValidate()
        {
            base.OnValidate();

            if (relicEffectCompound.relicEffect == null || relicEffectCompound.relicEffect.Count == 0)
            {
                relicEffectCompound.isPassive = true;
                return;
            }
            if (relicEffectCompound.relicEffect.Any(effect => effect == null)) return;

            relicEffectCompound.isPassive = relicEffectCompound.relicEffect.TrueForAll(effect => effect.IsPassive());
        }

#endif
    }
}
