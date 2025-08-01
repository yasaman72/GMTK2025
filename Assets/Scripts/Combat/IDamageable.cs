public interface IDamageable
{
    void TakeDamage(int damage);
    bool IsDead();
    int GetCurrentHealth();
}
