using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HealingPotionCard", menuName = "Cards/Healing Potion Card")]
    public class HealingPotionCard : BaseCard
    {
        [Header("Healing Properties")]
        public int healAmount = 5;
        public float moveSpeed = 1f;

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            callback += AfterCardActivated;
            runner.StopAllCoroutines();
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.3f);
            Transform playerHpPos = Player.PlayerCombatCharacter.HPOrigin;

            while (Vector2.Distance(cardPrefab.transform.position, playerHpPos.position) > 1)
            {
                cardPrefab.transform.position = Vector2.MoveTowards(
                    cardPrefab.transform.position,
                    playerHpPos.position,
                    0.05f * moveSpeed);
                yield return null;
            }

            Player.PlayerCombatCharacter.Heal(healAmount);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            callBack?.Invoke();
        }

#if UNITY_EDITOR
        private new void OnValidate()
        {
            base.OnValidate();
            var dict = new Dictionary<string, string>() { { "heal", healAmount.ToString() } };
            description.Arguments = new object[] { dict };
        }
#endif
    }
}