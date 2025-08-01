using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageDealer, IDamageable
{
    [SerializeField] private CombatParticipantStats _playerStats;

    public static IDamageable DamageableInstance { get; private set; }

    public void TakeDamage(int damage)
    {
        _playerStats.SetCurrentHealth(_playerStats.CurrentHealth - damage);
    }

    public bool IsDead()
    {
        return _playerStats.CurrentHealth <= 0;
    }

    public int GetCurrentHealth()
    {
        return _playerStats.CurrentHealth;
    }

    public void DealDamage(IDamageable target, int damage)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null");
            return;
        }
        target.TakeDamage(damage);
    }
}
