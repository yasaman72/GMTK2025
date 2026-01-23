using UnityEngine;
using UnistrokeGestureRecognition;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "GesturePatternBase", menuName = "Scriptable Objects/GesturePatternBase")]
    public class GesturePattern : GesturePatternBase
    {
        [SerializeField]
        private LassoShape _shape;
        public LassoShape Shape => _shape;
    }
}
