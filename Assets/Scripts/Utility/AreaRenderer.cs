using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Deviloop
{
    public class AreaRenderer : MonoBehaviour
    {
        [SerializeField] private bool shouldLog = true;
        [SerializeField] private SpriteShapeController _spriteShapeController;
        [SerializeField] private Animator _animator;
        Spline spline;

        private void Start()
        {
            spline = _spriteShapeController.spline;
            Clear();
        }

        public void RenderSpriteShape(List<Vector2> points)
        {
            if (points == null || points.Count < 3)
                return;

            points = FilterClosePoints(points);

            Vector2 center = GetCenter(points);
            _spriteShapeController.transform.position = center;

            spline.Clear();
            _spriteShapeController.gameObject.SetActive(true);
            spline.isOpenEnded = false;

            for (int i = 0; i < points.Count - 3; i++)
            {
                Vector2 localPoint = points[i] - center;

                spline.InsertPointAt(i, localPoint);
                spline.SetTangentMode(i, ShapeTangentMode.Linear);
            }


            _spriteShapeController.RefreshSpriteShape();

            _animator.SetTrigger("play");
        }

        private List<Vector2> FilterClosePoints(List<Vector2> points, float minDist = 0.02f)
        {
            List<Vector2> result = new();

            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0 || Vector2.Distance(points[i], result[^1]) > minDist)
                    result.Add(points[i]);
            }

            return result;
        }

        private Vector2 GetCenter(List<Vector2> points)
        {
            Vector2 sum = Vector2.zero;

            for (int i = 0; i < points.Count; i++)
                sum += points[i];

            return sum / points.Count;
        }

        public void Clear()
        {
            _spriteShapeController.gameObject.SetActive(false);
            spline.Clear();

            Logger.Log("Cleared SpriteShapeController", shouldLog);
        }
    }
}
