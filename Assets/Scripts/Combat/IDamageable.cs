using Deviloop.ScriptableObjects;

namespace Deviloop
{
    public interface IDamageable
    {
        void TakeDamage(int damage, AttackType type);
        bool IsDead();
        int GetCurrentHealth { get; }
        bool SetCurrentHealth(int health);
        void Heal(int amount);
        void FullyHeal();
        void AddShield(int amount);
        int DamageShield(int damageAmount);
    }
}
