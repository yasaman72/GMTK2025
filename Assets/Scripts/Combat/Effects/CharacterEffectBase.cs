using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Deviloop
{
    public abstract class CharacterEffectBase : ScriptableObject
    {
        [SerializeField] private LocalizedString _effectName;
        public LocalizedString EffectName => _effectName;
        [SerializeField] private Sprite _effectIcon;
        // TODO: need a better way to link relevant actions
        [SerializeField] protected List<EnemyAction> _relevantEnemyActions;

        public Sprite EffectIcon => _effectIcon;

        protected int _effectCurrentDuration = 0;


        public virtual bool CanBeApplied(EnemyAction enemyAction) => true;
        public abstract void OnApplyEffect(CombatCharacter character, out int remainedDuration);
        public virtual void OnAddEffect(CombatCharacter character, int duration)
        {
            _effectCurrentDuration += duration;
        }
        public virtual void OnRemoveEffect(CombatCharacter character)
        {
            _effectCurrentDuration = 0;
        }

        internal int GetRemainingDuration()
        {
            return _effectCurrentDuration;
        }
    }
}
