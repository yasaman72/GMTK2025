using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "CharacterEffect_AttackBuff", menuName = "Scriptable Objects/CharacterEffect/AttackBuff", order = 1)]
    public class AttackBuffCharacterEffect : CharacterEffectBase
    {
        [SerializeField] private int _attackBuffPower;

        public override bool CanBeApplied(EnemyAction enemyAction)
        {
            if (_relevantEnemyActions.Count <= 0 || _relevantEnemyActions.Contains(enemyAction))
                return true;

            return false;
        }

        public override void OnApplyEffect(CombatCharacter character, out int remainedDuration)
        {
            character.AddAttackBuff(_attackBuffPower);
            remainedDuration = --_effectCurrentDuration;
        }

        public override void OnRemoveEffect(CombatCharacter character)
        {
            character.RemoveAttackBuff(_attackBuffPower);
            base.OnRemoveEffect(character);
        }
    }
}
