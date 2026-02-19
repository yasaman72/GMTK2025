using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    public abstract class EnemyAction : ScriptableObject
    {
        public Sprite icon;
        public ModifiableFloat actionDelay = new ModifiableFloat(.5f);
        public ModifiableFloat actionDuration = new ModifiableFloat(1f);
        public LocalizedString translatedDescription;

        public virtual void TakeAction(IDamageDealer enemy, Action callback = null)
        {
            if (callback != null && enemy != null)
            {
                AfterAction(callback);
            }
        }

        protected async Task AfterAction(Action callback)
        {
            await Awaitable.WaitForSecondsAsync(actionDuration.Value);
            callback?.Invoke();
        }

        public abstract string IntentionNumber();
        public virtual bool CanBeTaken(EnemyAction previousAction) => true;
        protected virtual void ApplyOnEnable() { }
        protected virtual void ApplyOnValidate() { }

        protected void OnEnable()
        {
            ApplyOnEnable();
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            ApplyOnValidate();
        }
#endif
    }
}
