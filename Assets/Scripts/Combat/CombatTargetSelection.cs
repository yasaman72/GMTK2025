using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    public class CombatTargetSelection : MonoBehaviour
    {
        public static Action<CombatCharacter> SetTargetAction;
        public static CombatCharacter _currenTarget;
        public static CombatCharacter CurrentTarget
        {
            get => _currenTarget;
            private set
            {
                _currenTarget = value;
                // For debugging
                var debugComp = FindFirstObjectByType<CombatTargetSelection>();
                if (debugComp != null)
                {
                    debugComp._currentTargetDebug = _currenTarget != null ? _currenTarget.gameObject : null;
                }
            }
        }
        [SerializeField, ReadOnly] private GameObject _currentTargetDebug;
        [SerializeField] private GameObject _targetIndicator;

        private void OnEnable()
        {
            SetTargetAction += SetTarget;
        }

        private void OnDisable()
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
            if (CurrentTarget != null)
                (CurrentTarget as Enemy).OnDeath -= ClearTargetOnDeath;
            CurrentTarget = null;
            List<Enemy> aliveEnemies = CombatManager.SpawnedEnemies.Where(e => !e.IsDead()).ToList();

            // next target is the enemy with the lowest HP
            aliveEnemies.ToList().Sort((a, b) => a.GetCurrentHealth.CompareTo(b.GetCurrentHealth));
            if (aliveEnemies.Count > 0)
            {
                SetTarget(aliveEnemies[0]);
            }
            else
            {
                _targetIndicator.SetActive(false);
            }
        }
    }
}
