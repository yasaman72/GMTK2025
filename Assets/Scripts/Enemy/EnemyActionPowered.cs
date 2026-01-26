using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    public class EnemyActionPowered : EnemyAction
    {
        public int power = 10;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var dict = new Dictionary<string, string>() { { "ActionPower", power.ToString() } };
            translatedDescription.Arguments = new object[] { dict };
        }
#endif
    }
}
