using System;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyAction_Attack_[EnemyType]", menuName = "Scriptable Objects/EnemyActions/Attack", order = 1)]
    public class EnemyAction_Attack : EnemyActionPowered
    {
        public override void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
        {
            if (enemy == null)
            {
                Logger.LogWarning("Enemy is null");
                return;
            }

            IDamageable target = Player.PlayerCombatCharacter;
            if (target == null)
            {
                Logger.LogWarning("No valid target to attack.");
                return;
            }
            enemy.DealDamage(target, power);

            base.TakeAction(enemy, runner, callback);
        }
    }
}
