using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAction_Shield_[EnemyType]", menuName = "ScriptableObjects/EnemyActions/Shield", order = 1)]
public class EnemyAction_Shield : EnemyAction
{
    public override void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
    {
        (enemy as IDamageable).AddShield(power);

        base.TakeAction(enemy, runner, callback);
    }
}
