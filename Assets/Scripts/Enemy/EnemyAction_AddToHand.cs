using System;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "EnemyAction_Add[Item]ToHand_[EnemyType]", menuName = "Scriptable Objects/EnemyActions/AddToHand", order = 1)]
    public class EnemyAction_AddToHand : EnemyAction
    {
        public override void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
        {
            base.TakeAction(enemy, runner, callback);
        }
    }
}
