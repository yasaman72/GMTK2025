using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Relic", menuName = "Scriptable Objects/Relic")]
    public class Relic : ScriptableObject
    {
        public Action AfterApply;

        public bool isInGame = true;
        public string relicGUID;
        [Space]
        public LocalizedString relicName;
        [Space]
        public RelicEffectCompound relicEffectCompound;
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
            public bool isPassive;
            [SerializeReference, SubclassSelector]
            public BaseGameplayEvent gameplayEvent;

            [SerializeReference, SubclassSelector]
            public List<BasePredicate> predicates;

            [SerializeReference, SubclassSelector]
            public List<BaseRelicEffect> relicEffect;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetGuid();

            if (relicEffectCompound.relicEffect == null || relicEffectCompound.relicEffect.Count == 0)
            {
                relicEffectCompound.isPassive = true;
                return;
            }
            if (relicEffectCompound.relicEffect.Any(effect => effect == null)) return;

            relicEffectCompound.isPassive = relicEffectCompound.relicEffect.TrueForAll(effect => effect.IsPassive());
        }

        private void SetGuid()
        {
            string path = AssetDatabase.GetAssetPath(this);
            if (!string.IsNullOrEmpty(path))
            {
                relicGUID = AssetDatabase.AssetPathToGUID(path);
            }
        }
#endif
    }
}
