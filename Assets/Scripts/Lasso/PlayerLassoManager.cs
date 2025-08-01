using Cards;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLassoManager : MonoBehaviour
{
    [SerializeField] private bool shouldLog = true;
    [Space]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _pointSpacing = 0.1f;
    [SerializeField] private float _loopCloseThreshold = 0.5f;
    [SerializeField] private Gradient _defaultColor;
    [SerializeField] private Gradient _nearCloseColor;
    [Space]
    [SerializeField] private float _slowMotionTimeScale = 0.2f;

    private List<Vector2> _points = new();

    private void OnEnable()
    {
        if (_lineRenderer == null)
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
        ClearLasso();
    }

    private void Update()
    {
        if (!GameStateManager.CanPlayerDrawLasso) return;

        if (Input.GetMouseButtonDown(0))
        {
            ClearLasso();
            Time.timeScale = _slowMotionTimeScale;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_points.Count == 0 || Vector2.Distance(_points[^1], mousePos) > _pointSpacing)
            {
                _points.Add(mousePos);
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPosition(_points.Count - 1, mousePos);
            }

            if (IsLineNearOtherEnd())
            {
                CloseLoop();
                _lineRenderer.colorGradient = _nearCloseColor;
            }
            else
            {
                _lineRenderer.colorGradient = _defaultColor;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1f;

            if (IsLineNearOtherEnd())
            {
                CloseLoop();
                ClearLasso();
            }
            else
            {
                ClearLasso();
            }
        }
    }

    private void ClearLasso()
    {
        if (_lineRenderer == null)
        {
            Logger.LogWarning("LineRenderer is not assigned!");
            return;
        }

        _lineRenderer.positionCount = 0;
        _points.Clear();
        _lineRenderer.colorGradient = _defaultColor;
    }

    private bool IsLineNearOtherEnd()
    {
        return _points.Count > 3 && Vector2.Distance(_points[^1], _points[0]) < _loopCloseThreshold;
    }

    void CloseLoop()
    {
        Logger.Log("Loop closed!", shouldLog);
        Time.timeScale = 1f;
        _points.Add(_points[0]);
        _lineRenderer.positionCount = _points.Count;
        _lineRenderer.SetPosition(_points.Count - 1, _points[0]);

        DetectInsidePoints(_points);
    }

    void DetectInsidePoints(List<Vector2> loopPoints)
    {
        GameObject temp = new GameObject("TempCollider");
        PolygonCollider2D poly = temp.AddComponent<PolygonCollider2D>();
        poly.isTrigger = true;
        poly.points = loopPoints.ToArray();
        List<Collider2D> hits = new List<Collider2D>();
        Physics2D.OverlapCollider(poly, new ContactFilter2D().NoFilter(), hits);

        foreach (var hit in hits)
        {
            if (hit != null)
            {
                CardPrefab cardPrefab = hit.gameObject.GetComponent<CardPrefab>();
                if (cardPrefab)
                {
                    cardPrefab.OnLassoed();
                }

                Logger.Log($"Detected: {hit.gameObject.name}", shouldLog);
            }
        }

        Destroy(temp);
    }
}
