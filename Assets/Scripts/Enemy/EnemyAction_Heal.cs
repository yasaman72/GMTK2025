using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyAction_Heal_[EnemyType]", menuName = "Scriptable Objects/EnemyActions/Heal", order = 1)]
    public class EnemyAction_Heal : EnemyActionPowered
    {
        public override bool CanBeTaken(EnemyAction previousAction)
        {
            // don't heal if the previous action was also a heal
            if (previousAction is EnemyAction_Heal)
            {
                return false;
            }

            List<Enemy> aliveEnemies = CombatManager.SpawnedEnemies.Where(e => !e.IsDead()).ToList();
            // remove any enemy has full HP
            aliveEnemies.Where(e => e.GetCurrentHealth >= e.MaxHealth).ToList().ForEach(e => aliveEnemies.Remove(e));
            return aliveEnemies.Any();
        }

        public override void TakeAction(IDamageDealer enemy, Action callback = null)
        {
            // TODO: better logic for choosing who to heal
            List<Enemy> aliveEnemies = CombatManager.SpawnedEnemies.Where(e => !e.IsDead()).ToList();
            if (aliveEnemies.Count <= 0) return;

            aliveEnemies.Sort((a, b) => a.GetCurrentHealth.CompareTo(b.GetCurrentHealth));
            var targetToHeal = aliveEnemies[0];
            targetToHeal.Heal(power);

            base.TakeAction(enemy, callback);
        }
    }
}
