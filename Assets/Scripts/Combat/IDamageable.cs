public interface IDamageable
{
    void TakeDamage(int damage);
    bool IsDead();
    int GetCurrentHealth { get; }
    bool SetCurrentHealth(int health);
    void Heal(int amount);
    void AddShield(int amount);
    int DamageShield(int damageAmount);
}
