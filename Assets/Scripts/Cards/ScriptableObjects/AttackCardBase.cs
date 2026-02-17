using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AttackType
{
    Normal,
    Piercing,
}

namespace Deviloop.ScriptableObjects
{

    [CreateAssetMenu(fileName = "AttackCard_[attack name]", menuName = "Cards/Attack Card")]
    public class AttackCardBase : BaseCard
    {
        [Header("Attack Properties")]
        public int Damage = 3;
        public AttackType AttackType = AttackType.Normal;
        public ModifiableFloat MoveDuration = new ModifiableFloat(.2f);
        public ModifiableFloat DelayBeforeMove = new ModifiableFloat(.3f);
        public bool TaregtAll = false;
        public bool TargetRandom = false;
        public int DamagePlayer = 0;

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            callback += AfterCardActivated;

            runner.StopAllCoroutines();
            if (TaregtAll)
                runner.StartCoroutine(TargetAllEnemies(callback, cardPrefab));
            else
                runner.StartCoroutine(ActivateCardEffect(runner, callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            CombatCharacter enemy = null;

            if (TargetRandom)
            {
                enemy = CombatManager.Instance.GetRandomEnemy();
            }
            else
            {
                enemy = CombatTargetSelection.CurrentTarget;
            }

            if (enemy == null)
            {
                callback?.Invoke();
                yield break;
            }
            Transform enemyTransform = enemy.transform;

            yield return new WaitForSeconds(DelayBeforeMove.Value);

            DealDamageToPlayer();

            cardPrefab.transform.DOMove(enemyTransform.position, MoveDuration.Value).SetEase(Ease.Linear).OnUpdate(() =>
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
                if (runner != null)
                    runner.StartCoroutine(OnReachTarget(callback, enemy));
            });
        }

        private IEnumerator OnReachTarget(Action callback, CombatCharacter enemy)
        {
            Player.PlayerCombatCharacter.DealDamage(enemy, Damage, AttackType);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            yield return new WaitForSeconds(DelayBeforeMove.Value);
            callback?.Invoke();
        }

        // TODO: replace with async/await and proper animations
        private IEnumerator TargetAllEnemies(Action callback, CardPrefab cardPrefab)
        {
            List<Enemy> enemies = CombatManager.SpawnedEnemies.ToList();

            yield return new WaitForSeconds(DelayBeforeMove.Value);

            // make the card spin a bit with tweening before vanishing
            // TODO: better animations and wait for end of animation to apply effects
            var tween = cardPrefab.transform.DORotate(new Vector3(0, 0, 360 * 5), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);

            yield return new WaitForSeconds(DelayBeforeMove.Value);

            foreach (var enemy in enemies)
            {
                Player.PlayerCombatCharacter.DealDamage(enemy, Damage, AttackType);
            }

            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);

            yield return new WaitForSeconds(DelayBeforeMove.Value);
            tween.Kill();

            DealDamageToPlayer();

            callback?.Invoke();
        }

        private void DealDamageToPlayer()
        {
            if (DamagePlayer > 0)
            {
                IDamageable target = Player.PlayerCombatCharacter;
                target.TakeDamage(DamagePlayer, AttackType);
            }
        }

        private void OnEnable()
        {
            var dict = new Dictionary<string, string>() {
                { "damage", Damage.ToString() },
                { "playerDamage", DamagePlayer.ToString() }
            };
            description.Arguments = new object[] { dict };
        }
    }
}