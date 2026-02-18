using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Deviloop.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShieldCard", menuName = "Cards/Shield Card")]
    public class ShieldCard : BaseCard
    {
        [Header("Shield Properties")]
        public ModifiableFloat moveDuration = new ModifiableFloat(.2f);

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            callback += AfterCardActivated;
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.3f);

            Transform playerHpPos = FindAnyObjectByType<Player>().HPOrigin;

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