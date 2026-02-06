using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("DealDamageToAllEnemies")]
    [System.Serializable]
    public class DealDamageToAllEnemies : BaseRelicEffect
    {
        [SerializeField] private int _damage = 3;
        [SerializeField] private AttackType _attackType = AttackType.Normal;

        public override void Apply(MonoBehaviour caller)
        {
            var enemies = CombatManager.SpawnedEnemies;

            foreach (var enemy in enemies)
            {
                Player.PlayerCombatCharacter.DealDamage(enemy, _damage, _attackType);
            }

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
