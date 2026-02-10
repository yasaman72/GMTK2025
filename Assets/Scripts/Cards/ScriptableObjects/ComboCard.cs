using System;
using System.Collections;
using UnityEngine;

namespace Deviloop.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Combo", menuName = "Cards/Combo")]
    public class ComboCard : BaseCard
    {
        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            callback += AfterCardActivated;
            PlayerComboManager.OnCombo();
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.1f);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);
            callBack?.Invoke();
        }
    }
}
