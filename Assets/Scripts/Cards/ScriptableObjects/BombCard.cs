using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop.ScriptableObjects
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
            ModifiableFloat scaleDuration = new ModifiableFloat(0.5f);

            Vector3 targetScale = cardPrefab.transform.localScale * 2f;

            cardPrefab.transform.DOScale(targetScale, scaleDuration.Value);

            yield return new WaitForSeconds(scaleDuration.Value);

            Player.PlayerCombatCharacter.TakeDamage(damage, AttackType.Normal);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            callback?.Invoke();
        }

        private void OnEnable()
        {
            var dict = new Dictionary<string, string>() { { "damage", damage.ToString() } };
            description.Arguments = new object[] { dict };
        }
    }
}