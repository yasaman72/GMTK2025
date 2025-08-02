public interface IDamageable
{
    void TakeDamage(int damage);
    bool IsDead();
    int GetCurrentHealth();
    int MaxHealth { get; }
    void Heal(int amount);
    void AddShield(int amount);
}
