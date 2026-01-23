using System.Collections.Generic;
using UnistrokeGestureRecognition;
using UnityEngine;

namespace Deviloop
{
    public class GestureRecognizerController : MonoBehaviour
    {
        // Set of patterns for recognition
        [SerializeField] private List<GesturePattern> _patterns;

        private IGestureRecorder _gestureRecorder;
        private IGestureRecognizer<GesturePattern> _recognizer;


        private void Awake()
        {
            // Use this class to record the gesture path.
            // It uses a resampling algorithm to capture long paths to a limited size buffer.
            // The first value specifies the maximum number of points in the path buffer.
            // A higher value gives a more accurate path, but requires more memory.
            // The second value specifies the minimum distance between the previous and new point to record.
            // If the distance is less than required, the new point will not be added to the recorded path.
            // Useful when you want to exclude points from a path when the user keeps the cursor in one place.
            _gestureRecorder = new GestureRecorder(256, 0.1f);

            // Pass your patterns to the recognizer constructor.
            // You can also choose the number of points to resample.
            // A higher value gives more accurate results but takes longer to process.
            // 128 is the default and gives good results, but you can choose a better value for your pattern set.
            _recognizer = new GestureRecognizer<GesturePattern>(_patterns, 256);
        }

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

        public LassoShape RecordPoints(List<Vector2> points)
        {
            foreach (var p in points)
            {
                _gestureRecorder.RecordPoint(new Vector2(p.x, p.y));
            }

            var result = _recognizer.Recognize(_gestureRecorder.Path);

            Clear();

            if (result.Score < 0.8f)
                return LassoShape.Unknown;

            return result.Pattern.Shape;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying && _recognizer != null)
            {
                _recognizer.Dispose();
                _recognizer = new GestureRecognizer<GesturePattern>(_patterns);
            }
        }

#endif
    }
}
