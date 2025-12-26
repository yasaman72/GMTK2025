using Deviloop;
using FMODUnity;
using System;
using UnityEngine;

namespace Cards
{
    [System.Flags]
    public enum MaterialType
    {
        Unknown = 1 << 0,
        Metal = 1 << 1,
        Stone = 1 << 2,
        Wood = 1 << 3,
        Fabric = 1 << 4,
        Flesh = 1 << 5,
        Glass = 1 << 6,
        Explosive = 1 << 7
    }

    public enum Rarity
    {
        Common = 50,
        Uncommon = 30,
        Rare = 15,
        Epic = 4,
        Legendary = 1
    }

    [CreateAssetMenu(fileName = "BaseCard", menuName = "Cards/Base Card")]
    public abstract class BaseCard : ScriptableObject
    {
        public bool isInGame = true;
        [Header("Card Info")]
        public string cardName;
        public string description;
        public Sprite cardIcon;
        public Rarity cardRarity;
        public MaterialType materialType;
        public bool isNegativeItem;
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
    }
}