using System;
using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("Shield")]
    [System.Serializable]
    public class EnemyAction_Shield : EnemyActionPowered
    {
        public override void TakeAction(IDamageDealer enemy, Action callback = null)
        {
            (enemy as IDamageable).AddShield(power);

            base.TakeAction(enemy, callback);
        }
    }
}
