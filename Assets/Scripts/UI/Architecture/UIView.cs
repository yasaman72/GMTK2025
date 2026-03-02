using UnityEngine;

namespace Deviloop
{
    public abstract class UIView : MonoBehaviour, IInitiatable
    {
        public bool OpenIsolated = false;
        public abstract bool Open();
        public abstract void Close();
        public virtual void Initiate() { }
        public virtual void Deactivate() { }
    }
}
