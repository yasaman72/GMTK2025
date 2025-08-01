using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBehavior_Attack_", menuName = "ScriptableObjects/EnemyBehavior/Attack", order = 1)]
public class EnemyBehavior_Attack : EnemyBehavior
{
    public int attackDamage = 10;

    public override void TakeAction(IDamageDealer enemy)
    {
        if (enemy == null)
        {
            Logger.LogWarning("Enemy is null");
            return;
        }

        IDamageable target = PlayerManager.DamageableInstance;
        if (target == null)
        {
            Logger.LogWarning("No valid target to attack.");
            return;
        }
        enemy.DealDamage(target, attackDamage);
    }
}
