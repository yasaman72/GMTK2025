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

        private void OnEnable()
        {
            SetTargetAction += SetTarget;
        }

        private void OnDisable()
        {
            SetTargetAction += SetTarget;
        }

        private void SetTarget(CombatCharacter target)
        {
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
            _targetIndicator.transform.DOMove(target.transform.position + Vector3.up * 1.5f, .2f);
            (target as Enemy).OnDeath += ClearTargetOnDeath;
        }

        private void ClearTargetOnDeath(CombatCharacter character)
        {
            (CurrentTarget as Enemy).OnDeath -= ClearTargetOnDeath;
            SetTarget(null);
        }
    }
}
