using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageDealer, IDamageable
{
    public static Action<int, int> OnPlayerHPChanged;
    [SerializeField] private CombatParticipantStats _playerStats;

    public static IDamageable DamageableInstance { get; private set; }

    private void Awake()
    {
        DamageableInstance = this;
        _playerStats.ResetHp();
    }

    private void OnEnable()
    {
        OnPlayerHPChanged?.Invoke(_playerStats.CurrentHealth, _playerStats.MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        _playerStats.SetCurrentHealth(_playerStats.CurrentHealth - damage);
        OnPlayerHPChanged?.Invoke(_playerStats.CurrentHealth, _playerStats.MaxHealth);
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            Logger.LogWarning("Heal amount cannot be negative");
            return;
        }
        _playerStats.SetCurrentHealth(_playerStats.CurrentHealth + amount);
        OnPlayerHPChanged?.Invoke(_playerStats.CurrentHealth, _playerStats.MaxHealth);
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
