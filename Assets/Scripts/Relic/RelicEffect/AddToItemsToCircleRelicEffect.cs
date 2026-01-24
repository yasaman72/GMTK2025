
using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("AddToItemsToCircle")]
    [System.Serializable]
    public class AddToItemsToCircle : BaseRelicEffect
    {
        public int amountToAdd = 1;

        public override void Apply(MonoBehaviour caller)
        {
        }

        public override void OnAdded()
        {
            PlayerLassoManager.MaxedAllowedItems += amountToAdd;
        }

        public override void OnRemoved()
        {
            PlayerLassoManager.MaxedAllowedItems -= amountToAdd;
        }

        public override bool IsPassive() => true;
    }
}
