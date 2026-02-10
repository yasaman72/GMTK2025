using Deviloop;
using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "BaseCard", menuName = "Cards/Base Card")]
    public abstract class BaseCard : GUIDScriptableObject
    {
        public bool isInGame = true;
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

        [Header("Prefab")]
        public GameObject cardPrefab;

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