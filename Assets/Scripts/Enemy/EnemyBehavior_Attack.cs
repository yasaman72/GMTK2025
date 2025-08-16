using Cards;
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBehavior_Attack_", menuName = "ScriptableObjects/EnemyBehavior/Attack", order = 1)]
public class EnemyBehavior_Attack : EnemyBehavior
{
    public int attackDamage = 10;
    public int attackDuration = 2;

    public override void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
    {
        if (enemy == null)
        {
            Logger.LogWarning("Enemy is null");
            return;
        }

        IDamageable target = PlayerManager.PlayerDamageableInstance;
        if (target == null)
        {
            Logger.LogWarning("No valid target to attack.");
            return;
        }
        enemy.DealDamage(target, attackDamage);


        if (runner == null )
        {
            callback?.Invoke();
            return;

        }

        if (callback != null)
        {
            runner.StartCoroutine(ActivateCardEffect(callback));
        }
    }
    private IEnumerator ActivateCardEffect(Action callback)
    {
        yield return new WaitForSeconds(attackDuration);
        callback?.Invoke();
    }

}
