using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Deviloop
{
    public abstract class CharacterEffectBase : ScriptableObject
    {
        [SerializeField] private LocalizedString _effectName;
        public LocalizedString EffectName => _effectName;

        [SerializeField] protected LocalizedString _description;
        public LocalizedString Description => _description;

        [SerializeField] private Sprite _effectIcon;
        public Sprite EffectIcon => _effectIcon;

        [SerializeField] private Color _effectColor;
        public Color EffectColor => _effectColor;
        // TODO: need a better way to link relevant actions
        [SerializeField] protected List<EnemyAction> _relevantEnemyActions;

        protected int _effectCurrentDuration = 0;
        protected int EffectCurrentDuration
        {
            get => _effectCurrentDuration;
            set
            {
                _effectCurrentDuration = value;
                (_description.Arguments[0] as Dictionary<string, string>)["duration"] = value.ToString();
            }
        }

        public virtual bool CanBeApplied(EnemyAction enemyAction) => true;
        public abstract void OnApplyEffect(CombatCharacter character, out int remainedDuration);
        public virtual void OnAddEffect(CombatCharacter character, int duration)
        {
            EffectCurrentDuration += duration;
        }
        public virtual void OnRemoveEffect(CombatCharacter character)
        {
            EffectCurrentDuration = 0;
        }

        internal int GetRemainingDuration()
        {
            return EffectCurrentDuration;
        }

        public void ModifyDuration(int duration)
        {
            EffectCurrentDuration += duration;
        }

        protected void OnEnable()
        {
            AttackBuffCharacterEffect attackEffect = this as AttackBuffCharacterEffect;
            BleedEffect bleedEffect = this as BleedEffect;

            Dictionary<string, string> dict = new Dictionary<string, string>() {
                { "duration", EffectCurrentDuration.ToString() },
                { "attackBuff", attackEffect?.AttackBuffPower.ToString() },
                { "bleedPower", bleedEffect?.BleedPower.ToString() },
            };

            _description.Arguments = new object[] { dict };
        }
    }
}
