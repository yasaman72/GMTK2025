using Deviloop.ScriptableObjects;

public interface IDamageDealer
{
    void DealDamage(IDamageable target, int damage, AttackType type);
}
