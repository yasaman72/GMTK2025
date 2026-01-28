using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("DealDamageToTarget")]
    [System.Serializable]
    public class DealDamageToTarget : BaseRelicEffect
    {
        [SerializeField] private int _damage = 3;

        public override void Apply(MonoBehaviour caller)
        {
            var enemy = CombatTargetSelection.CurrentTarget;
            Player.PlayerCombatCharacter.DealDamage(enemy, _damage);
        }

        public override void OnAdded()
        {
        }

        public override void OnRemoved()
        {
        }

        public override bool IsPassive() => false;
    }
}
