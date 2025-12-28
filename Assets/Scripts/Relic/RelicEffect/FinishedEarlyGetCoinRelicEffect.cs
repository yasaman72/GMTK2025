using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "FinishedEarlyGetCoinRelicEffect", menuName = "Scriptable Objects/Relic Effects/Finished Early Get Coin")]
    public class FinishedEarlyGetCoinRelicEffect : BaseRelicEffect
    {
        [SerializeField] private int _coinAmount = 10;

        public override void Apply(MonoBehaviour caller)
        {
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
