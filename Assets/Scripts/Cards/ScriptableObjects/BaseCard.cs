using Deviloop;
using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Cards
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

        [Header("Card Properties")]
        public bool isConsumable = false;

        // Virtual method for card effects - can be overridden
        public virtual void OnCardActivated(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            UseCard(runner, callback, cardPrefab);

            if (isConsumable)
            {
                // TODO: Implement logic to consume card
                Logger.Log($"{cardName} consumed and removed from deck", shouldLog);
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
                Rarity.Common => Mathf.Max(30, price),
                Rarity.Uncommon => Mathf.Max(70, price),
                Rarity.Rare => Mathf.Max(100, price),
                Rarity.Legendary => Mathf.Max(120, price),
                _ => price,
            };
        }
#endif
    }
}