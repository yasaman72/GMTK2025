using System.Collections.Generic;

namespace Deviloop
{
    public abstract class EnemyActionPowered : EnemyAction
    {
        public int power = 10;

        public override string IntentionNumber()
        {
            return power.ToString();
        }

        public override void OnActionSelected()
        {
            var dict = new Dictionary<string, string>() { { "ActionPower", power.ToString() } };
            translatedDescription.Arguments = new object[] { dict };
        }
    }
}
