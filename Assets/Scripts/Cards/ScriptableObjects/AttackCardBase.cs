using UnityEngine;
using System.Collections;
using System;

namespace Cards.ScriptableObjects
{
    public abstract class AttackCardBase : BaseCard
    {
        [Header("Axe Properties")]
        public int damage = 3;
        public int moveSpeed = 1;
        public float delayBeforeMove = 0.3f;

        public override void UseCard(MonoBehaviour runner, Action callBack, CardPrefab cardPrefab)
        {
            Debug.Log($"Dagger deals {damage} damage to selected enemy");
            runner.StopAllCoroutines();
            runner.StartCoroutine(ActivateCardEffect(callBack, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            // Visuals and animation
            var enemy = FindAnyObjectByType<EnemyBrain>();
            Transform enemyTransform = enemy.transform;

            yield return new WaitForSeconds(delayBeforeMove);

            while (Vector2.Distance(cardPrefab.transform.position, enemyTransform.position) > 1)
            {
                cardPrefab.transform.position = Vector2.MoveTowards(
                    cardPrefab.transform.position,
                    enemyTransform.position,
                    0.05f * moveSpeed);
                yield return null;
            }

            enemy.GetComponentInChildren<DamageIndicatorApplier>()?.ShowDamageIndicator(damage);

            // TODO: update the target selection if more enemies present in one comabt (milestone 2)
            PlayerManager.PlayerDamageDealerInstance.DealDamage(enemy, damage);

            yield return new WaitForSeconds(1);
            callBack?.Invoke();
        }
    }
}