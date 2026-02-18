
using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public abstract class CardAnimationType
    {
        public abstract void Play(GameObject target);
    }

    [AddTypeMenu("MoveToTarget")]
    [System.Serializable]
    public class MoveToTarget : CardAnimationType
    {

        public override void Play(GameObject target)
        {

        }
    }

}
