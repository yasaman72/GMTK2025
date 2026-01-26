using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    public class EnemyActionPowered : EnemyAction
    {
        public int power = 10;

        public override string IntentionNumber()
        {
            return power.ToString();
        }

        protected override void ApplyOnValidate()
        {
            var dict = new Dictionary<string, string>() { { "ActionPower", power.ToString() } };
            translatedDescription.Arguments = new object[] { dict };
        }
    }
}
