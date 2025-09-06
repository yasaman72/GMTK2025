using FMODUnity;
using System;
using UnityEngine;

public class CombatCharacter : Character, IDamageDealer, IDamageable
{
    public Action OnHPChanged;
    public Action OnShieldChanged;
    public Action OnDamageRecieved;
    public Action<CombatCharacter> OnDeath;

    [SerializeField] public CombatCharacterStats Stats;
    [SerializeField] private DamageIndicatorApplier _damageIndicatorApplier;
    [SerializeField] public Transform HPOrigin;
    [Header("Audio")]
    [SerializeField] private EventReference _hitSound;
    [SerializeField] private EventReference _onHitShieldSound;
    [SerializeField] private EventReference _deathSound;

    [SerializeField, ReadOnly] private int CurrentHealth;
    [SerializeField, ReadOnly] private int CurrentShield;

    public int MaxHealth => Stats.MaxHealth;
    public int GetCurrentHealth => CurrentHealth;
    public int GetCurrentShield => CurrentShield;

    protected virtual void Start()
    {
        ResetStats();
        OnHPChanged?.Invoke();
    }

    public void ResetStats()
    {
        CurrentHealth = MaxHealth;
        CurrentShield = 0;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead())
        {
            return;
        }

        int finalDamage = DamageShield(damage);
        OnShieldChanged?.Invoke();

        if (finalDamage != damage)
        {
            AudioManager.PlayAudioOneShot?.Invoke(_onHitShieldSound);
        }

        if (finalDamage > 0)
        {
            AudioManager.PlayAudioOneShot?.Invoke(_hitSound);
            OnDamageRecieved?.Invoke();
        }

        bool isDead = SetCurrentHealth(CurrentHealth - finalDamage);
        OnHPChanged?.Invoke();
        _damageIndicatorApplier.ShowDamageIndicator(finalDamage);

        if (isDead)
        {
            AudioManager.PlayAudioOneShot?.Invoke(_deathSound);
            OnDeath?.Invoke(this);
        }
    }

    public int DamageShield(int damageAmount)
    {
        if (CurrentShield >= damageAmount)
        {
            CurrentShield -= damageAmount;
            return 0;
        }
        else
        {
            int remainingDamage = damageAmount - CurrentShield;
            CurrentShield = 0;
            return remainingDamage;
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            Logger.LogWarning("Heal amount cannot be negative");
            return;
        }
        SetCurrentHealth(CurrentHealth + amount);
        OnHPChanged?.Invoke();
    }

    public bool SetCurrentHealth(int health)
    {
        CurrentHealth = Mathf.Clamp(health, 0, MaxHealth);
        return CurrentHealth <= 0;
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
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
        CurrentShield += amount;
        OnShieldChanged?.Invoke();
    }
}
