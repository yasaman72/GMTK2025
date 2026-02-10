using Deviloop;
using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("AddToItemsThrown")]
    [System.Serializable]
    public class AddToItemsThrown : BaseRelicEffect
    {
        [SerializeField] private int _amountToAdd = 1;
        public override bool IsPassive() => true;

        public override void Apply(MonoBehaviour caller)
        {
        }

        public override void OnAdded()
        {
            CardManager.CardsToThrowPerTurn += _amountToAdd;
        }

        public override void OnRemoved()
        {
            CardManager.CardsToThrowPerTurn -= _amountToAdd;
        }
    }
}
