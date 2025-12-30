using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public abstract class BaseRelicEffect
    {
        public abstract void Apply(MonoBehaviour caller);

        public abstract void OnAdded();
        public abstract void OnRemoved();
    }
}
