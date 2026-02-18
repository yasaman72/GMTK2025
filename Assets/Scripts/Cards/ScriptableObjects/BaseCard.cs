using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "BaseCard", menuName = "Cards/Base Card")]
    public abstract class BaseCard : GUIDScriptableObject
    {
        [SerializeField, SerializeReference, SubclassSelector]
        protected List<CardEffect> _cardEffects;
        [Tooltip("will be applied in order")]
        [SerializeField, SerializeReference, SubclassSelector]
        protected CardAnimationType[] _animationType;
        [Space]
        public bool isInGame = true;
        public Vector2 spriteScale = new Vector2(.5f, .5f);
        [Header("Card Info")]
        public LocalizedString cardName;
        public LocalizedString description;
        public Sprite cardIcon;
        public Rarity rarity;
        public F_MaterialType materialType;
        public bool isNegative;

        [Space]
        public int price;
        [SerializeField] private bool overridePrice = false;

        [Space]
        public Color OnSelectColor = Color.green;
        public EventReference OnUseSound;
        [SerializeField] protected bool shouldLog;

        [DeveloperNotes, SerializeField]
        private string developerNotes;

        // TODO: update the consumable cards logic to consume before adding the card back to discard deck
        // TODO: try adding usage time to consumables
        [Header("Card Properties")]
        public bool isConsumable = false;

        // Virtual method for card effects - can be overridden
        public virtual void OnCardActivated(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            UseCard(runner, callback, cardPrefab);
        }

        // TODO: a better architecture to remove the consumables
        public virtual void AfterCardActivated()
        {
            if (isConsumable)
            {
                CardManager.RemoveCardFromDeckAction?.Invoke(this, true);
            }
        }

        // Abstract method that each card type must implement
        protected abstract void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab);

        protected void ApplyEffects(CombatCharacter target = null)
        {
            foreach (var effect in _cardEffects)
            {
                effect.Apply(target);
            }
        }
        protected void OnEnable()
        {
            DamagePlayer damagePlayerEffect = (DamagePlayer)_cardEffects.Find(e => e is DamagePlayer);
            DamageEnemyCardEffect damagingEffect = (DamageEnemyCardEffect)_cardEffects.Find(e => e is DamageEnemyCardEffect);
            HealPlayerEffect healEffect = (HealPlayerEffect)_cardEffects.Find(e => e is HealPlayerEffect);
            ShieldPlayerEffect shieldEffect = (ShieldPlayerEffect)_cardEffects.Find(e => e is ShieldPlayerEffect);
            List<AddCharacterEffect> addEffects = _cardEffects
                .FindAll(e => e is AddCharacterEffect)
                .ConvertAll(e => (AddCharacterEffect)e);

            var dict = new Dictionary<string, string>() {
                { "damage", damagingEffect?.damage.ToString() },
                { "playerDamage", damagePlayerEffect?.damage.ToString() },
                { "heal", healEffect?.healAmount.ToString() },
                { "shield", shieldEffect?.shieldAmount.ToString() },
                { $"effect{1}Duration", addEffects.Count > 0 ? addEffects[0].duration.ToString() : null },
                { $"effect{2}Duration", addEffects.Count > 1 ? addEffects[1].duration.ToString() : null },
                { $"effect{3}Duration", addEffects.Count > 2 ? addEffects[2].duration.ToString() : null },
                { $"effect{4}Duration", addEffects.Count > 3 ? addEffects[3].duration.ToString() : null },
            };
            description.Arguments = new object[] { dict };
        }

#if UNITY_EDITOR
        protected new void OnValidate()
        {
            base.OnValidate();

            if (overridePrice) return;

            price = rarity switch
            {
                Rarity.Common => Mathf.Max(20, price),
                Rarity.Uncommon => Mathf.Max(40, price),
                Rarity.Rare => Mathf.Max(60, price),
                Rarity.Legendary => Mathf.Max(80, price),
                _ => price,
            };
        }
#endif
    }
}