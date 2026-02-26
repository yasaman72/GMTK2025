using FMODUnity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "[name]_Card", menuName = "Cards/Card")]
    public class BaseCard : GUIDScriptableObject
    {
        [SerializeField] protected bool shouldLog;
        public int activationOrder = 10;

        [Header("Effects")]
        [SerializeField, SerializeReference, SubclassSelector]
        protected List<CardEffect> _cardEffects;
        [SerializeField, SerializeReference, SubclassSelector]
        protected List<CardEffect> _onLassoEffects;
        [SerializeField, SerializeReference, SubclassSelector]
        protected List<CardEffect> _onDropEffects;
        [Tooltip("will be applied in order")]
        [SerializeField, SerializeReference, SubclassSelector]
        protected CardAnimationType[] _animationType;

        [SerializeField] private GameObject _extentionPrefab;
        [SerializeField] private bool TargetRandom = false;
        // TODO: add usage time to consumables
        public bool isConsumable = false;
        public bool isNegative;

        [Header("General Properties")]
        public bool isInGame = true;
        public Rarity rarity;
        public F_MaterialType materialType;

        [Header("Price")]
        public int price;
        [SerializeField] private bool overridePrice = false;

        [Header("Visuals")]
        public Sprite cardIcon;
        public Vector2 spriteScale = new Vector2(.5f, .5f);
        public Color OnSelectColor = Color.green;
        public LocalizedString cardName;
        public LocalizedString description;

        [Header("Audio")]
        [SerializeField] private EventReference OnUseSound;

        [DeveloperNotes, SerializeField]
        private string developerNotes;


        public void AddComponent(GameObject cardPrefab)
        {
            if (_extentionPrefab == null) return;

            if (cardPrefab.GetComponentAtIndex(0).name == _extentionPrefab.name)
            {
                return;
            }

            GameObject instance = Instantiate(_extentionPrefab);
            instance.transform.SetParent(cardPrefab.transform, true);
            instance.transform.localPosition = Vector3.zero;
        }


        // Abstract method that each card type must implement
        public async Task UseCard(Action callback, CardPrefab cardPrefab)
        {
            CombatCharacter enemy = GetTargetEnemy();

            if (enemy == null)
            {
                callback?.Invoke();
                return;
            }

            await PlayAnimations(cardPrefab.gameObject, enemy.gameObject);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            if (enemy == null || enemy.IsDead())
            {
                enemy = CombatTargetSelection.CurrentTarget;

                if (enemy == null)
                {
                    callback?.Invoke();
                    return;
                }
            }

            ApplyOnGrabEffects(enemy, cardPrefab);
            callback?.Invoke();

            // TODO: a better architecture to remove the consumables
            if (isConsumable)
            {
                CardManager.RemoveCardFromDeckAction?.Invoke(this, true);
            }
        }

        protected async Task PlayAnimations(GameObject card, GameObject target = null)
        {
            for (int i = 0; i < _animationType.Length; i++)
            {
                CardAnimationType animation = _animationType[i];
                if (i != _animationType.Length - 1 && animation.playWithNext)
                {
                    animation.Play(card, target);
                    continue;
                }

                await animation.Play(card, target);
            }
        }

        protected void ApplyOnGrabEffects(CombatCharacter target, CardPrefab cardPrefab)
        {
            foreach (var effect in _cardEffects)
            {
                effect.Apply(target, cardPrefab);
            }
        }

        public void ApplyOnLassoeEffects(CardPrefab cardPrefab)
        {
            CombatCharacter target = GetTargetEnemy();
            foreach (var effect in _onLassoEffects)
            {
                effect.Apply(target, cardPrefab);
            }
        }

        public void ApplyOnDropEffects(CardPrefab cardPrefab)
        {
            CombatCharacter target = GetTargetEnemy();

            foreach (var effect in _onDropEffects)
            {
                effect.Apply(target, cardPrefab);
            }
        }

        private CombatCharacter GetTargetEnemy()
        {
            CombatCharacter enemy = null;

            if (TargetRandom)
            {
                enemy = CombatManager.Instance.GetRandomEnemy();
            }
            else
            {
                enemy = CombatTargetSelection.CurrentTarget;
            }

            if (enemy.IsDead())
            {
                return null;
            }

            return enemy;
        }

        [ContextMenu("update smart localized texts")]
        protected void OnEnable()
        {
            DamagePlayer damagePlayerEffect = (DamagePlayer)_cardEffects.Find(e => e is DamagePlayer);
            DamageEnemyCardEffect damagingEffect = (DamageEnemyCardEffect)_cardEffects.Find(e => e is DamageEnemyCardEffect);
            HealPlayerEffect healEffect = (HealPlayerEffect)_cardEffects.Find(e => e is HealPlayerEffect) ?? 
                                          (HealPlayerEffect)_onDropEffects.Find(e => e is HealPlayerEffect) ??
                                          (HealPlayerEffect)_onLassoEffects.Find(e => e is HealPlayerEffect);
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

            if (GameDataBaseManager.GameDatabase == null) return;

            foreach (var config in GameDataBaseManager.GameDatabase.rarityConfigs)
            {
                if (config.rarity == rarity)
                {
                    price = config.basePrice;
                    break;
                }
            }
        }
#endif
    }
}