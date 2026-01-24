using System;
using System.Collections;
using System.Collections.Generic;
using UnistrokeGestureRecognition;
using UnityEngine;

namespace Deviloop
{
    public class GestureRecognizerController : MonoBehaviour
    {
        [SerializeField] private List<GesturePattern> _patterns;
        [SerializeField] private bool _logResults = true;
        [Header("Config")]
        [SerializeField] private int _recorderPathMaxLength = 128;
        [SerializeField] private float _newPointMinDistance = 0.2f;
        [SerializeField] private int _resamplePointsNumber = 128;

        private IGestureRecorder _gestureRecorder;
        private IGestureRecognizer<GesturePattern> _recognizer;

        private void OnDestroy()
        {
            _recognizer.Dispose();
            _gestureRecorder.Dispose();
        }

        private void Clear()
        {
            // Clear the gesture recorder buffer for the new path
            _gestureRecorder.Reset();
        }

        public IEnumerator RecordPoints(
            List<Vector2> points,
            Action<LassoShape> onResult)
        {
            var patternsCopy = new List<GesturePattern>(_patterns);
            _gestureRecorder = new GestureRecorder(_recorderPathMaxLength, _newPointMinDistance);

            for (int i = 0; i < points.Count; i++)
            {
                Vector2 p = points[i];
                var Vec = new Vector2(p.x, p.y);
                _gestureRecorder.RecordPoint(Vec);
            }

            yield return StartCoroutine(RecognizeShape(points, patternsCopy, onResult));
        }

        private IEnumerator RecognizeShape(
            List<Vector2> points,
            List<GesturePattern> patterns,
            Action<LassoShape> onResult)
        {
            // You can also choose the number of points to resample.
            // A higher value gives more accurate results but takes longer to process.
            // 128 is the default and gives good results, but you can choose a better value for your pattern set.
            _recognizer = new GestureRecognizer<GesturePattern>(patterns, _resamplePointsNumber);

            var result = _recognizer.Recognize(_gestureRecorder.Path);

            yield return null;

            if (result.Score < result.Pattern.ScoreAccuracy)
            {
                Logger.Log($"failed shape: {result.Pattern.Shape}, score: {result.Score}", _logResults);

                if (patterns.Count > 1)
                {
                    patterns.Remove(result.Pattern);
                    yield return StartCoroutine(
                        RecognizeShape(points, patterns, onResult));
                    yield break;
                }

                onResult?.Invoke(LassoShape.Unknown);
                yield break;
            }

            Clear();
            onResult?.Invoke(result.Pattern.Shape);
        }
    }
}
