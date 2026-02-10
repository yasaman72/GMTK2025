using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HealingPotionCard", menuName = "Cards/Healing Potion Card")]
    public class HealingPotionCard : BaseCard
    {
        [Header("Healing Properties")]
        public int healAmount = 5;
        public float moveDuration = .5f;

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

            cardPrefab.transform.DOMove(playerHpPos.position, moveDuration).SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        Player.PlayerCombatCharacter.Heal(healAmount);
                        AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

                        callBack?.Invoke();
                    });
        }

        private void OnEnable()
        {
            var dict = new Dictionary<string, string>() { { "heal", healAmount.ToString() } };
            description.Arguments = new object[] { dict };
        }
    }
}