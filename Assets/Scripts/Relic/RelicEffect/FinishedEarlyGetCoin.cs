using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("FinishedEarlyGetCoin")]
    [System.Serializable]
    public class FinishedEarlyGetCoin : BaseRelicEffect
    {
        [SerializeField] private int _coinAmount = 10;

        public override void Apply(MonoBehaviour caller)
        {
            // TODO: extract the predicates
            if (CombatManager.CombatRoundCounter <= 3)
                PlayerInventory.CoinCount += _coinAmount;
        }

        public override void OnAdded()
        {
        }

        public override void OnRemoved()
        {
        }
    }
}
