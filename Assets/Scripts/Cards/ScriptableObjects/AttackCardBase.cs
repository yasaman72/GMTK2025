using Deviloop;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AttackCard_[attack name]", menuName = "Cards/Attack Card")]
    public class AttackCardBase : BaseCard
    {
        [Header("Attack Properties")]
        public int damage = 3;
        public float moveDuration = .5f;
        public float delayBeforeMove = 0.3f;
        public bool taregtAll = false;

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            callback += AfterCardActivated;

            runner.StopAllCoroutines();
            if (taregtAll)
                runner.StartCoroutine(TargetAllEnemies(callback, cardPrefab));
            else
                runner.StartCoroutine(ActivateCardEffect(runner, callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            var enemy = CombatTargetSelection.CurrentTarget;
            if (enemy == null)
            {
                callback?.Invoke();
                yield break;
            }
            Transform enemyTransform = enemy.transform;

            yield return new WaitForSeconds(delayBeforeMove);

            cardPrefab.transform.DOMove(enemyTransform.position, moveDuration).SetEase(Ease.Linear).OnUpdate(() =>
            {
                if (enemy == null || enemy.IsDead())
                {
                    enemy = CombatTargetSelection.CurrentTarget;
                    if (enemy == null)
                    {
                        callback?.Invoke();
                        cardPrefab.transform.DOKill();
                        return;
                    }
                    enemyTransform = enemy.transform;
                }
            })
            .OnComplete(
            () =>
            {
                runner.StartCoroutine(OnReachTarget(callback, enemy));
            });
        }

        private IEnumerator OnReachTarget(Action callback, CombatCharacter enemy)
        {
            Player.PlayerCombatCharacter.DealDamage(enemy, damage);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            yield return new WaitForSeconds(1);
            callback?.Invoke();
        }

        private IEnumerator TargetAllEnemies(Action callback, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(delayBeforeMove);

            // make the card spin a bit with tweening before vanishing
            // TODO: better animations and wait for end of animation to apply effects
            var tween = cardPrefab.transform.DORotate(new Vector3(0, 0, 360 * 5), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);

            yield return new WaitForSeconds(delayBeforeMove * 2);

            var enemies = CombatManager.SpawnedEnemies;

            foreach (var enemy in enemies)
            {
                Player.PlayerCombatCharacter.DealDamage(enemy, damage);
            }

            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            yield return new WaitForSeconds(1);
            tween.Kill();
            callback?.Invoke();
        }

        private void OnEnable()
        {
            var dict = new Dictionary<string, string>() { { "damage", damage.ToString() } };
            description.Arguments = new object[] { dict };
        }
    }
}