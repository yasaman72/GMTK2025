using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    public abstract class EnemyAction : ScriptableObject
    {
        public Sprite icon;
        public int actionDelay = 1;
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

        public abstract string IntentionNumber();
        public virtual bool CanBeTaken() => true;
        protected virtual void ApplyOnValidate() { }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyOnValidate();
        }
#endif
    }
}
