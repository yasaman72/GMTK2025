using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BombCard", menuName = "Cards/Bomb Card")]
    public class BombCard : BaseCard
    {
        [Header("Bomb Properties")]
        public int damage = 3;

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            callback += AfterCardActivated;
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callback, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.1f);

            float scaleDuration = 0.5f;
            Vector3 targetScale = cardPrefab.transform.localScale * 2f;

            cardPrefab.transform.DOScale(targetScale, scaleDuration);

            yield return new WaitForSeconds(scaleDuration);

            Player.PlayerCombatCharacter.TakeDamage(damage);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            callback?.Invoke();
        }

#if UNITY_EDITOR

        private new void OnValidate()
        {
            base.OnValidate();
            var dict = new Dictionary<string, string>() { { "damage", damage.ToString() } };
            description.Arguments = new object[] { dict };
        }
#endif
    }
}