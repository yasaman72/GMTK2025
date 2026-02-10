using Deviloop;
using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("ThrowAtOnce")]
    [System.Serializable]
    public class ThrowAtOnce : BaseRelicEffect
    {
        public override bool IsPassive() => true;

        public override void Apply(MonoBehaviour caller)
        {
        }

        public override void OnAdded()
        {
            CardManager.ShouldThrowAtOnce = true;
        }

        public override void OnRemoved()
        {
            CardManager.ShouldThrowAtOnce = false;
        }
    }
}
