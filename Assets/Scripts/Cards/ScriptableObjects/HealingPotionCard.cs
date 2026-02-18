using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Deviloop.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HealingPotionCard", menuName = "Cards/Healing Potion Card")]
    public class HealingPotionCard : BaseCard
    {
        [Header("Healing Properties")]
        public ModifiableFloat moveDuration = new ModifiableFloat(.2f);

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

            cardPrefab.transform.DOMove(playerHpPos.position, moveDuration.Value).SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        ApplyEffects();
                        
                        AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);
                        callBack?.Invoke();
                    });
        }
    }
}