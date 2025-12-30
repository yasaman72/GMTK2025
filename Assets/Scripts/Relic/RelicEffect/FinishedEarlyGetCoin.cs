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
