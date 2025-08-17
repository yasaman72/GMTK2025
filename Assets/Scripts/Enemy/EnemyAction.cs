using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyAction : ScriptableObject
{
    public Sprite icon;
    public int power = 10;
    public int actionDuration = 2;

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
}
