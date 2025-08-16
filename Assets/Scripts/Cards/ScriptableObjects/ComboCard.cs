using System;
using System.Collections;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Combo", menuName = "Cards/Combo")]
    public class ComboCard : BaseCard
    {
        public override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            PlayerComboManager.OnCombo();
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.1f);
            callBack?.Invoke();
        }
    }
}
