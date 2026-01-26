using System;
using UnityEngine;

namespace Deviloop
{
[CreateAssetMenu(fileName = "EnemyAction_Heal_[EnemyType]", menuName = "Scriptable Objects/EnemyActions/Heal", order = 1)]
    public class EnemyAction_Heal : EnemyActionPowered
    {
        public override void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
        {
            base.TakeAction(enemy, runner, callback);

        }
    }
}
