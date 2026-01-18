
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
            PlayerLassoManager.maxedAllowedItems += amountToAdd;
        }

        public override void OnRemoved()
        {
            PlayerLassoManager.maxedAllowedItems -= amountToAdd;
        }

        public override bool IsPassive() => true;
    }
}
