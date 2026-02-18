using Deviloop.ScriptableObjects;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
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
        private List<CharacterEffectBase> _effectsToRemove = new List<CharacterEffectBase>();

        protected virtual void Start()
        {
            ResetStats();
            OnHPChanged?.Invoke();
        }

        protected void OnEnable()
        {
            TurnManager.OnTurnChanged += OnTurnChanged;
        }

        protected void OnDisable()
        {
            TurnManager.OnTurnChanged -= OnTurnChanged;
        }

        public void ResetStats()
        {
            _currentHealth = MaxHealth;
            _currentShield = 0;
        }

        public virtual void TakeDamage(int damage, AttackType type)
        {
            if (IsDead())
            {
                return;
            }

            int finalDamage = type == AttackType.Piercing ? damage : DamageShield(damage);
            int shieldDamage = damage - finalDamage;

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

            if (shieldDamage > 0)
            {
                _damageIndicatorApplier.ShowDamageIndicator(shieldDamage, DamageType.Shield);
            }
            if (finalDamage > 0)
            {
                _damageIndicatorApplier.ShowDamageIndicator(finalDamage);
            }

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
                OnShieldChanged?.Invoke();
                return 0;
            }
            else
            {
                int remainingDamage = damageAmount - _currentShield;
                _currentShield = 0;
                OnShieldChanged?.Invoke();
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

        public virtual void DealDamage(IDamageable target, int damage, AttackType type)
        {
            if (target == null)
            {
                Debug.LogWarning("Target is null");
                return;
            }
            target.TakeDamage(damage + _currentAttackBuff, type);
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
        // TODO: better implementation for removing effects
        private void OnTurnChanged(TurnManager.ETurnMode mode)
        {
            if (mode == TurnManager.ETurnMode.Player)
            {
                foreach (var effect in _effectsToRemove)
                {
                    effect.OnRemoveEffect(this);
                    _currentEffects.Remove(effect);
                    RemoveEffectIcon(effect);
                }
                _effectsToRemove.Clear();
            }
        }

        public void AddEffect(CharacterEffectBase effect, int duration)
        {
            if (effect == null)
            {
                Logger.LogWarning("Effect is null", shouldLog);
                return;
            }

            if (GetEffect(effect, out CharacterEffectBase result))
            {
                // if already has the effect, increas the duration and update UI
                result.ModifyDuration(duration);
                UpdateEffectIcons();
            }
            else
            {
                // create a copy of the effect to avoid shared state issues
                var effectCopy = Instantiate(effect);
                effectCopy.OnAddEffect(this, duration);
                _currentEffects.Add(effectCopy);

                AddEffectIcon(effectCopy, duration);
            }

        }

        public void RemoveEffect(CharacterEffectBase effect)
        {
            if (effect == null)
            {
                Logger.LogWarning("Effect is null", shouldLog);
                return;
            }
            _effectsToRemove.Add(effect);

        }

        public void RemoveAllEffects()
        {
            foreach (var effect in _currentEffects)
            {
                effect.OnRemoveEffect(this);
                RemoveEffectIcon(effect);
            }
            _currentEffects.Clear();
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
                iconComponent.Effect = effect;
            }
        }

        private void RemoveEffectIcon(CharacterEffectBase effect)
        {
            foreach (Transform child in _effectsHolder)
            {
                var iconComponent = child.GetComponent<EffectIcon>();
                if (iconComponent != null && iconComponent.Effect == effect)
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
                    CharacterEffectBase effect = iconComponent.Effect;
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

        public bool GetEffect(CharacterEffectBase effect, out CharacterEffectBase result)
        {
            foreach (CharacterEffectBase e in _currentEffects)
            {
                if (e.GetType() == effect.GetType())
                {
                    result = e;
                    return true;
                }
            }

            result = null;
            return false;
        }

        #endregion
    }
}