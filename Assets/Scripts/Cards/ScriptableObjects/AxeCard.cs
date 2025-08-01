using System;
using System.Collections;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AxeCard", menuName = "Cards/Axe Card")]
    public class AxeCard : BaseCard
    {
        [Header("Axe Properties")]
        public int damage = 3;

        public override void UseCard(MonoBehaviour runner, Action callBack)
        {
            Debug.Log($"Axe deals {damage} damage to selected enemy");
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