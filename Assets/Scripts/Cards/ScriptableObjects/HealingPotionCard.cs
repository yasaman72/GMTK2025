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
        public float moveSpeed = 1f;

        private void Awake()
        {
            isConsumable = true; // Potions are always consumable
        }

        public override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            Logger.Log($"Healing Potion heals player for {healAmount} HP", shouldLog);
            runner.StopAllCoroutines();
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.3f);
            Transform playerHpPos = FindAnyObjectByType<PlayerManager>()._playerHpOrigin;

            while (Vector2.Distance(cardPrefab.transform.position, playerHpPos.position) > 1)
            {
                cardPrefab.transform.position = Vector2.MoveTowards(
                    cardPrefab.transform.position,
                    playerHpPos.position,
                    0.05f * moveSpeed);
                yield return null;
            }

            PlayerManager.PlayerDamageableInstance.Heal(healAmount);
            AudioManager.OnPlaySoundEffct?.Invoke(onUseSound);

            callBack?.Invoke();
        }
    }
}