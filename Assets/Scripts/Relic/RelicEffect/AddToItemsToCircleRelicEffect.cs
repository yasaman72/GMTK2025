
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "AddToItemsToCircleRelicEffect", menuName = "Scriptable Objects/Relic Effects/Add to Items to Circle")]
    public class AddToItemsToCircleRelicEffect : BaseRelicEffect
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
    }
}
