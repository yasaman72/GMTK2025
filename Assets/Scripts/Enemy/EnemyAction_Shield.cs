using System;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyAction_Shield_[EnemyType]", menuName = "Scriptable Objects/EnemyActions/Shield", order = 1)]
    public class EnemyAction_Shield : EnemyActionPowered
    {
        public override void TakeAction(IDamageDealer enemy, Action callback = null)
        {
            (enemy as IDamageable).AddShield(power);

            base.TakeAction(enemy, callback);
        }
    }
}
