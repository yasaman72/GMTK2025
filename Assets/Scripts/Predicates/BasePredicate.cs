using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public abstract class BasePredicate
    {
        public abstract bool Check();
    }

    [AddTypeMenu("CollectedCardsCountEqual")]
    [System.Serializable]
    public class CollectedCardsCountEqual :BasePredicate
    {
        [SerializeField]
        private int _amountToMatch = 1;

        public override bool Check()
        {
            return PlayerLassoManager.lassoedCardsCount == _amountToMatch;
        }
    }

    [AddTypeMenu("CombatTurnCountLessThanOrEqual")]
    [System.Serializable]
    public class CombatTurnCountLessThanOrEqual : BasePredicate
    {
        [SerializeField]
        private int _amountToMatch = 3;

        public override bool Check()
        {
            return CombatManager.CombatRoundCounter <= _amountToMatch;
        }
    }

    [AddTypeMenu("ShapeRecognizedEquals")]
    [System.Serializable]
    public class ShapeRecognizedEquals : BasePredicate
    {
        [SerializeField]
        private LassoShape _shape;

        public override bool Check()
        {
            return PlayerLassoManager.recordedLassoShape == _shape;
        }
    }
}
