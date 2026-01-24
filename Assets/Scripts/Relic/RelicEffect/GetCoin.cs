using UnityEngine;

namespace Deviloop
{
    [AddTypeMenu("GetCoin")]
    [System.Serializable]
    public class GetCoin : BaseRelicEffect
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

        public override bool IsPassive() => false;
    }
}
