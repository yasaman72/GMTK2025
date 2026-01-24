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
        [SerializeField, Range(0, 1)] private float _scoreAccuracy = 0.8f;
        public float ScoreAccuracy => _scoreAccuracy;
    }
}
