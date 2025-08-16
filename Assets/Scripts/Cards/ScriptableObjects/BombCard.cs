using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BombCard", menuName = "Cards/Bomb Card")]
    public class BombCard : BaseCard
    {
        [Header("Bomb Properties")]
        public int damage = 3;

        public override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            Logger.Log($"Bomb explodes! Player takes {damage} damage", shouldLog);
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callback, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.1f);

            float scaleDuration = 0.5f;
            Vector3 targetScale = cardPrefab.transform.localScale * 2f;

            cardPrefab.transform.DOScale(targetScale, scaleDuration);

            yield return new WaitForSeconds(scaleDuration);

            PlayerManager.PlayerDamageableInstance.TakeDamage(damage);
            AudioManager.OnPlaySoundEffct?.Invoke(onUseSound);

            callback?.Invoke();
        }
    }
}