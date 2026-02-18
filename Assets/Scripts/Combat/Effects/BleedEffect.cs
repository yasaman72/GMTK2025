using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "CharacterEffect_Bleed", menuName = "Scriptable Objects/CharacterEffect/Bleed", order = 1)]
    public class BleedEffect : CharacterEffectBase
    {
        [SerializeField] private int _bleedPower;
        public int BleedPower => _bleedPower;

        public override bool CanBeApplied(EnemyAction enemyAction)
        {
            return true;
        }

        public override void OnApplyEffect(CombatCharacter character, out int remainedDuration)
        {
            character.TakeDamage(_bleedPower, AttackType.Piercing);
            remainedDuration = --EffectCurrentDuration;
        }

        public override void OnRemoveEffect(CombatCharacter character)
        {
            base.OnRemoveEffect(character);
        }
    }
}
