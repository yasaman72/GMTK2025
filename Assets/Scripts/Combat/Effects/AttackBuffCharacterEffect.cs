using System.Linq;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "CharacterEffect_AttackBuff", menuName = "Scriptable Objects/CharacterEffect/AttackBuff", order = 1)]
    public class AttackBuffCharacterEffect : CharacterEffectBase
    {
        [SerializeField] private int _attackBuffPower;
        public int AttackBuffPower => _attackBuffPower;

        public override bool CanBeApplied(EnemyAction enemyAction)
        {
            if (_relevantEnemyActions.Count <= 0 || _relevantEnemyActions.Any(a => a.GetType() == enemyAction.GetType()))
                return true;

            return false;
        }

        public override void OnAddEffect(CombatCharacter character, int duration)
        {
            base.OnAddEffect(character, duration);
            character.AddAttackBuff(_attackBuffPower);
        }

        public override void OnApplyEffect(CombatCharacter character, out int remainedDuration)
        {
            remainedDuration = --EffectCurrentDuration;
        }

        public override void OnRemoveEffect(CombatCharacter character)
        {
            character.RemoveAttackBuff(_attackBuffPower);
            base.OnRemoveEffect(character);
        }
    }
}
