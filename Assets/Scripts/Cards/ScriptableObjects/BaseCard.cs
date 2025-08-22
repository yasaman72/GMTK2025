using FMODUnity;
using System;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BaseCard", menuName = "Cards/Base Card")]
    public abstract class BaseCard : ScriptableObject
    {
        [Header("Card Info")]
        public string cardName;
        public string description;
        public Sprite cardIcon;
        public bool isNegativeItem;
        public Color OnSelectColor = Color.green;
        public EventReference OnUseSound;
        [SerializeField] protected bool shouldLog;

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