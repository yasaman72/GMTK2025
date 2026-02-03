using System;
using UnityEngine;
using DG.Tweening;

namespace Deviloop
{
    public class CombatTargetSelection : MonoBehaviour
    {
        public static Action<CombatCharacter> SetTargetAction;
        public static CombatCharacter CurrentTarget { get; private set; }
        [SerializeField] private GameObject _targetIndicator;

        private void Start()
        {
            SetTargetAction += SetTarget;
        }

        private void OnDestroy()
        {
            SetTargetAction -= SetTarget;
        }

        private void SetTarget(CombatCharacter target)
        {
            if (gameObject == null) return;

            if (CurrentTarget != null)
            {
                (CurrentTarget as Enemy).OnDeath -= ClearTargetOnDeath;
            }

            CurrentTarget = target;
            if (CurrentTarget == null)
            {
                _targetIndicator.SetActive(false);
                return;
            }

            _targetIndicator.SetActive(true);
            _targetIndicator.transform.DOMove(target.transform.position + (Vector3.up * 2.5f), .2f);
            (target as Enemy).OnDeath += ClearTargetOnDeath;
        }

        private void ClearTargetOnDeath(CombatCharacter enemy)
        {
            (CurrentTarget as Enemy).OnDeath -= ClearTargetOnDeath;
            CurrentTarget = null;
            var allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            Enemy nextTarget = null;
            foreach (var e in allEnemies)
            {
                if (!e.IsDead())
                {
                    nextTarget = e;
                    break;
                }
            }
            SetTarget(nextTarget ? nextTarget : null);
        }
    }
}
