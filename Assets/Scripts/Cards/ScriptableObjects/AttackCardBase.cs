using UnityEngine;
using System.Collections;
using System;
using Deviloop;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AttackCard_[attack name]", menuName = "Cards/Attack Card")]
    public class AttackCardBase : BaseCard
    {
        [Header("Attack Properties")]
        public int damage = 3;
        public int moveSpeed = 1;
        public float delayBeforeMove = 0.3f;

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            runner.StopAllCoroutines();
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callback, CardPrefab cardPrefab)
        {
            // Visuals and animation
            var enemy = CombatTargetSelection.CurrentTarget;
            if (enemy == null)
            {
                callback?.Invoke();
                yield break;
            }
            Transform enemyTransform = enemy.transform;

            yield return new WaitForSeconds(delayBeforeMove);

            while (Vector2.Distance(cardPrefab.transform.position, enemyTransform.position) > 1)
            {
                if (enemy == null || enemy.IsDead())
                {
                    callback?.Invoke();
                    yield break;
                }

                cardPrefab.transform.position = Vector2.MoveTowards(
                    cardPrefab.transform.position,
                    enemyTransform.position,
                    0.05f * moveSpeed);
                yield return null;
            }

            enemy.GetComponentInChildren<DamageIndicatorApplier>()?.ShowDamageIndicator(damage);

            // TODO: update the target selection if more enemies present in one comabt (milestone 2)
            Player.PlayerCombatCharacter.DealDamage(enemy, damage);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            yield return new WaitForSeconds(1);
            callback?.Invoke();
        }
    }
}