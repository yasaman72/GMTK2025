using System;
using System.Collections;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShieldCard", menuName = "Cards/Shield Card")]
    public class ShieldCard : BaseCard
    {
        [Header("Shield Properties")]
        public int shieldAmount = 1;

        public override void UseCard(MonoBehaviour runner, Action callBack)
        {
            Debug.Log($"Shield provides {shieldAmount} protection");
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