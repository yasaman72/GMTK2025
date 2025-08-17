using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageDealer, IDamageable
{
    public static Action<int, int> OnPlayerHPChanged;
    public static Action<int> OnPlayerShieldChanged;
    public static Action OnPlayerDeath;

    [SerializeField] private CombatParticipantStats _playerStats;
    [SerializeField] private DamageIndicatorApplier _damageIndicatorApplier;
    [SerializeField] public Transform _playerHpOrigin;
    [Header("Audio")]
    [SerializeField] private AudioClip _playerHitSound;
    [SerializeField] private AudioClip _onHitShieldSound;
    [SerializeField] private AudioClip _playerDeathSound;

    public static IDamageable PlayerDamageableInstance { get; private set; }
    public static IDamageDealer PlayerDamageDealerInstance { get; private set; }

    public int MaxHealth => _playerStats.MaxHealth;

    private void Awake()
    {
        PlayerDamageableInstance = this;
        PlayerDamageDealerInstance = this;
        _playerStats.ResetStats();
    }

    private void OnEnable()
    {
        OnPlayerHPChanged?.Invoke(_playerStats.CurrentHealth, _playerStats.MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = _playerStats.DamageShield(damage);
        OnPlayerShieldChanged?.Invoke(_playerStats.CurrentShield);

        if (finalDamage != damage)
        {
            AudioManager.OnPlaySoundEffct?.Invoke(_onHitShieldSound);
        }

        if (finalDamage > 0)
        {
            AudioManager.OnPlaySoundEffct?.Invoke(_playerHitSound);
        }

        bool isDead = _playerStats.SetCurrentHealth(_playerStats.CurrentHealth - finalDamage);
        OnPlayerHPChanged?.Invoke(_playerStats.CurrentHealth, _playerStats.MaxHealth);
        _damageIndicatorApplier.ShowDamageIndicator(finalDamage);

        if (isDead)
        {
            AudioManager.OnPlaySoundEffct?.Invoke(_playerDeathSound);
            OnPlayerDeath?.Invoke();
        }
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

    public void AddShield(int amount)
    {
        _playerStats.AddShield(amount);
        OnPlayerShieldChanged?.Invoke(_playerStats.CurrentShield);
    }
}
