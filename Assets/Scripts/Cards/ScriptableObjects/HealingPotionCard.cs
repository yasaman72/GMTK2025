using System;
using System.Collections;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HealingPotionCard", menuName = "Cards/Healing Potion Card")]
    public class HealingPotionCard : BaseCard
    {
        [Header("Healing Properties")]
        public int healAmount = 5;
    
        private void Awake()
        {
            isConsumable = true; // Potions are always consumable
        }

        public override void UseCard(MonoBehaviour runner, Action callBack, CardPrefab cardPrefab)
        {
            Debug.Log($"Healing Potion heals player for {healAmount} HP");
            runner.StopAllCoroutines();
            runner.StartCoroutine(ActivateCardEffect(callBack));
        }

        private IEnumerator ActivateCardEffect(Action callBack)
        {
            // TODO: implement effects and logic
            yield return new WaitForSeconds(1);
            callBack?.Invoke();
        }
    }
}