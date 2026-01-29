using Deviloop;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : Character, IDamageDealer, IDamageable, IEffectReceiver
{
    public Action OnHPChanged;
    public delegate void AttackBuffAppliedHandler(bool isApplied);
    public AttackBuffAppliedHandler OnAttackBuffApplied;
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
    [Header("Effect")]
    [SerializeField] private GameObject _effectUIIcon;
    [SerializeField] private Transform _effectsHolder;

    [SerializeField, ReadOnly] private int _currentHealth;
    [SerializeField, ReadOnly] private int _currentShield;
    [SerializeField, ReadOnly] private List<CharacterEffectBase> _currentEffects;
    [SerializeField, ReadOnly] private int _currentAttackBuff;
    public int CurrentAttackBuff => _currentAttackBuff;

    public int ExtraMaxHealth { get; set; } = 0;
    public int MaxHealth => Stats.MaxHealth + ExtraMaxHealth;
    public int GetCurrentHealth => _currentHealth;
    public int GetCurrentShield => _currentShield;
    public List<CharacterEffectBase> GetCurrentEffects => _currentEffects;

    protected virtual void Start()
    {
        ResetStats();
        OnHPChanged?.Invoke();
    }

    public void ResetStats()
    {
        _currentHealth = MaxHealth;
        _currentShield = 0;
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

        bool isDead = SetCurrentHealth(_currentHealth - finalDamage);
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
        if (_currentShield >= damageAmount)
        {
            _currentShield -= damageAmount;
            return 0;
        }
        else
        {
            int remainingDamage = damageAmount - _currentShield;
            _currentShield = 0;
            return remainingDamage;
        }
    }

    public virtual void Heal(int amount)
    {
        if (amount < 0)
        {
            Logger.LogWarning("Heal amount cannot be negative");
            return;
        }
        SetCurrentHealth(_currentHealth + amount);
    }

    public void FullyHeal()
    {
        SetCurrentHealth(MaxHealth);
    }

    public bool SetCurrentHealth(int health)
    {
        _currentHealth = Mathf.Clamp(health, 0, MaxHealth);
        OnHPChanged?.Invoke();
        return _currentHealth <= 0;
    }

    public bool IsDead()
    {
        return _currentHealth <= 0;
    }

    public virtual void DealDamage(IDamageable target, int damage)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null");
            return;
        }
        target.TakeDamage(damage + _currentAttackBuff);
    }

    public void AddShield(int amount)
    {
        _currentShield += amount;
        OnShieldChanged?.Invoke();
    }

    protected void RemoveAllShields()
    {
        _currentShield = 0;
        OnShieldChanged?.Invoke();
    }

    // TODO: replace architecture with composition over inheritance
    #region Effects
    public void AddEffect(CharacterEffectBase effect, int duration)
    {
        if (effect == null)
        {
            Logger.LogWarning("Effect is null", shouldLog);
            return;
        }
        // create a copy of the effect to avoid shared state issues
        var effectCopy = Instantiate(effect);
        effectCopy.OnAddEffect(this, duration);
        _currentEffects.Add(effectCopy);

        AddEffectIcon(effectCopy, duration);
    }

    public void RemoveEffect(CharacterEffectBase effect)
    {
        if (effect == null)
        {
            Logger.LogWarning("Effect is null", shouldLog);
            return;
        }
        effect.OnRemoveEffect(this);
        _currentEffects.Remove(effect);
        RemoveEffectIcon(effect);
    }

    public void ApplyAllEffects(EnemyAction enemyAction)
    {
        int effectsCount = _currentEffects.Count;
        if (effectsCount <= 0)
        {
            return;
        }

        for (int i = effectsCount - 1; i >= 0; i--)
        {
            var _currentEffect = _currentEffects[i];
            if (_currentEffect.CanBeApplied(enemyAction))
            {
                ApplyEffect(_currentEffects[i]);
            }
        }
    }

    public void ApplyEffect(CharacterEffectBase effect)
    {
        if (effect == null)
        {
            Logger.LogWarning("Effect is null", shouldLog);
            return;
        }
        int remainedDuration;
        effect.OnApplyEffect(this, out remainedDuration);
        if (remainedDuration <= 0)
        {
            RemoveEffect(effect);
            return;
        }

        UpdateEffectIcons();
    }

    private void AddEffectIcon(CharacterEffectBase effect, int duration)
    {
        var iconObject = Instantiate(_effectUIIcon, _effectsHolder);
        var iconComponent = iconObject.GetComponent<EffectIcon>();
        if (iconComponent != null)
        {
            iconComponent.Initialize(effect, duration);
            iconComponent.AssociatedEffect = effect;
        }
    }

    private void RemoveEffectIcon(CharacterEffectBase effect)
    {
        foreach (Transform child in _effectsHolder)
        {
            var iconComponent = child.GetComponent<EffectIcon>();
            if (iconComponent != null && iconComponent.AssociatedEffect == effect)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    private void UpdateEffectIcons()
    {
        foreach (Transform child in _effectsHolder)
        {
            var iconComponent = child.GetComponent<EffectIcon>();
            if (iconComponent != null)
            {
                var effect = iconComponent.AssociatedEffect;
                int duration = effect.GetRemainingDuration();
                iconComponent.UpdateDurationText(duration);
            }
        }
    }

    public void AddAttackBuff(int amount)
    {
        _currentAttackBuff = amount;
        OnAttackBuffApplied?.Invoke(true);
    }

    public void RemoveAttackBuff(int amount)
    {
        _currentAttackBuff -= amount;
        OnAttackBuffApplied?.Invoke(false);
    }

    #endregion
}
