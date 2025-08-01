using System;
using System.Collections;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BombCard", menuName = "Cards/Bomb Card")]
    public class BombCard : BaseCard
    {
        [Header("Bomb Properties")]
        public int damage = 3;

        public override void UseCard(MonoBehaviour runner, Action callBack, CardPrefab cardPrefab)
        {
            Debug.Log($"Bomb explodes! Player takes {damage} damage");
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