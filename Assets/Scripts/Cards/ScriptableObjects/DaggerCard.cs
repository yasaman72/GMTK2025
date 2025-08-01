using System;
using System.Collections;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DaggerCard", menuName = "Cards/Dagger Card")]
    public class DaggerCard : BaseCard
    {
        [Header("Dagger Properties")]
        public int damage = 1;

        public override void UseCard(MonoBehaviour runner, Action callBack)
        {
            Debug.Log($"Dagger deals {damage} damage to selected enemy");
            runner.StartCoroutine(ActivateCardEffect(callBack));
        }

        private IEnumerator ActivateCardEffect(Action callBack)
        {
            // TODO: implement effects and logic
            yield return new WaitForSeconds(1);
            callBack?.Invoke();
        }
    }
}