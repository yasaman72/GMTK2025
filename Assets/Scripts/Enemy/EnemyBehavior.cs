using System;
using UnityEngine;

public abstract class EnemyBehavior : ScriptableObject
{
    public Sprite icon;
    public abstract void TakeAction(IDamageDealer enemy, MonoBehaviour runner = null, Action callback = null);
}
