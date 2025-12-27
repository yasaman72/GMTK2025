using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public abstract class EnemyAction : ScriptableObject
{
    public Sprite icon;
    public int power = 10;
    public int actionDuration = 2;
    // TODO: turn this into smart localized string that can take parameters like power, etc.
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

    private void OnValidate()
    {
        var dict = new Dictionary<string, string>() { { "ActionPower",  power.ToString()} };
        translatedDescription.Arguments = new object[] { dict };
    }
}
