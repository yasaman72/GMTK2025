using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShieldCard", menuName = "Cards/Shield Card")]
    public class ShieldCard : BaseCard
    {
        [Header("Shield Properties")]
        public int shieldAmount = 1;
        public float moveDuration = .5f;

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            callback += AfterCardActivated;
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.3f);

            Transform playerHpPos = FindAnyObjectByType<Player>().HPOrigin;

            cardPrefab.transform.DOMove(playerHpPos.position, moveDuration).SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        Player.PlayerCombatCharacter.AddShield(shieldAmount);
                        AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

                        callBack?.Invoke();
                    });
        }

#if UNITY_EDITOR
        private new void OnValidate()
        {
            base.OnValidate();

            var dict = new Dictionary<string, string>() { { "shield", shieldAmount.ToString() } };
            description.Arguments = new object[] { dict };
        }
#endif
    }
}