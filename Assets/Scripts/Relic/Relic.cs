using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Relic", menuName = "Scriptable Objects/Relic")]
    public class Relic : ScriptableObject
    {
        public string relicName;
        public Sprite icon;
        [TextArea]
        public string description;
    }
}
