using UnityEngine;

namespace Deviloop
{
    // TODO: replace scriptable object with normal class if possible. need to create editor drawer to show the effects
    [System.Serializable]
    public abstract class BaseRelicEffect: ScriptableObject
    {
        public abstract void Apply(MonoBehaviour caller);

        public abstract void OnAdded();
        public abstract void OnRemoved();
    }
}
