using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    public abstract class EnemyAction : ScriptableObject
    {
        public Sprite icon;
        public int actionDuration = 2;
        public LocalizedString translatedDescription;

        public virtual void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null)
        {
            if (runner == null)
            {
                callback?.Invoke();
                return;

            }

            if (callback != null)
            {
                runner.StartCoroutine(AfterAction(callback));
            }
        }

        protected virtual IEnumerator AfterAction(Action callback)
        {
            yield return new WaitForSeconds(actionDuration);
            callback?.Invoke();
        }

        public abstract int IntentionNumber();
    }
}
