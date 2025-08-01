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

        [Header("Prefab")]
        public GameObject cardPrefab; // The prefab that will be thrown (with rigidbody)

        [Header("Card Properties")]
        public bool isConsumable = false; // Whether this card is removed after use

        // Abstract method that each card type must implement
        public abstract void UseCard(MonoBehaviour runner, Action callBack, CardPrefab cardPrefab);

        // Virtual method for card effects - can be overridden
        public virtual void OnCardActivated(MonoBehaviour runner, Action callBack, CardPrefab cardPrefab)
        {
            UseCard(runner, callBack, cardPrefab);

            if (isConsumable)
            {
                Debug.Log($"{cardName} consumed and removed from deck");
            }
        }
    }
}