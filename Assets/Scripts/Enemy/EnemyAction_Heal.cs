using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyAction_Heal_[EnemyType]", menuName = "Scriptable Objects/EnemyActions/Heal", order = 1)]
    public class EnemyAction_Heal : EnemyActionPowered
    {
        public override void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
        {
            // TODO: better logic for choosing who to heal
            List<Enemy> enemyAsEnemy = CombatManager.SpawnedEnemies.Where(e => !e.IsDead()).ToList();
            enemyAsEnemy.Sort((a, b) => a.GetCurrentHealth.CompareTo(b.GetCurrentHealth));
            var targetToHeal = enemyAsEnemy[0];
            targetToHeal.Heal(power);

            base.TakeAction(enemy, runner, callback);
        }
    }
}
