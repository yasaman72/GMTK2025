using Cards;
using Deviloop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLassoManager : MonoBehaviour
{
    public static Action<LassoShape> OnLassoShapeRecognized;
    [SerializeField] private bool shouldLog = true;
    [Space]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private ContactFilter2D _itemsFilter;
    [SerializeField] private float _pointSpacing = 0.1f;
    [SerializeField] private float _loopCloseThreshold = 0.5f;
    [SerializeField] private Gradient _defaultColor;
    [SerializeField] private Gradient _nearCloseColor;
    [Space]
    [SerializeField] private float _slowMotionTimeScale = 0.2f;
    [SerializeField] private int _maxAllowedItems = 3;
    [SerializeField] private ParticleSystem _spellParticleSystem;
    [SerializeField] private GestureRecognizerController _gestureRecognizerController;

    private List<Vector2> _points = new();
    private bool _hasAlreadyDrawn = false;
    private bool _startNewLine = false;

    // TODO: a more generic to handle values modified by relics
    public static int maxedAllowedItems;
    public static int lassoedCardsCount = 0;
    public static LassoShape recordedLassoShape = LassoShape.Unknown;

    private void Start()
    {
        maxedAllowedItems = _maxAllowedItems;
    }

    private void OnEnable()
    {
        if (_lineRenderer == null)
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
        ClearLasso();
        GameStateManager.OnPlayerClickedThrowButton += OnPlayerDrawTurnStart;
        TurnManager.OnTurnChanged += OnTurnChanged;

        _spellParticleSystem.Stop();
        _spellParticleSystem.gameObject.SetActive(false);
        _spellParticleSystem.transform.position = Vector2.one * 1000;
    }

    private void OnDisable()
    {
        GameStateManager.OnPlayerClickedThrowButton -= OnPlayerDrawTurnStart;
        TurnManager.OnTurnChanged -= OnTurnChanged;
    }


    private void OnTurnChanged(TurnManager.ETurnMode turnMode)
    {
        if (turnMode != TurnManager.ETurnMode.Player)
            ClearLasso();
    }

    private void OnPlayerDrawTurnStart()
    {
        ClearLasso();
        _hasAlreadyDrawn = false;
    }

    private void Update()
    {
        if (!GameStateManager.CanPlayerDrawLasso || _hasAlreadyDrawn)
        {
            Time.timeScale = 1f;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _startNewLine = true;
            Time.timeScale = _slowMotionTimeScale;
        }

        if (Input.GetMouseButton(0))
        {
            if (!_startNewLine) return;

            _spellParticleSystem.gameObject.SetActive(true);
            if (!_spellParticleSystem.isPlaying)
                _spellParticleSystem.Play();

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _spellParticleSystem.transform.position = mousePos;
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
            _startNewLine = false;
            Time.timeScale = 1f;

            if (IsLineNearOtherEnd())
            {
                CloseLoop();
            }
            else
            {
                ClearLasso();
            }
        }
    }

    private void ClearLasso()
    {
        _startNewLine = false;

        _spellParticleSystem.Stop();
        _spellParticleSystem.gameObject.SetActive(false);
        _spellParticleSystem.transform.position = Vector2.one * 1000;

        _lineRenderer.positionCount = 0;
        _points.Clear();
        _lineRenderer.colorGradient = _defaultColor;
    }

    private bool IsLineNearOtherEnd()
    {
        return _points.Count > 10 && Vector2.Distance(_points[^1], _points[0]) < _loopCloseThreshold;
    }

    void CloseLoop()
    {
        Time.timeScale = 1f;

        _points.Add(_points[0]);
        _lineRenderer.positionCount = _points.Count;
        _lineRenderer.SetPosition(_points.Count - 1, _points[0]);

        StartCoroutine(DetectInsidePoints(_points));
    }

    private IEnumerator DetectInsidePoints(List<Vector2> loopPoints)
    {
        GameObject temp = new GameObject("TempCollider");
        PolygonCollider2D poly = temp.AddComponent<PolygonCollider2D>();
        poly.isTrigger = true;
        poly.points = loopPoints.ToArray();
        List<Collider2D> hits = new List<Collider2D>();
        Physics2D.OverlapCollider(poly, _itemsFilter, hits);

        List<CardPrefab> lassoedCards = new List<CardPrefab>();

        if (hits.Count <= 0 || hits.Count > maxedAllowedItems)
        {
            if (hits.Count > maxedAllowedItems)
                MessageController.OnDisplayMessage?.Invoke($"Max allowed items is {maxedAllowedItems}.", 2);

            ClearLasso();
            Time.timeScale = 1f;
            Destroy(temp);
            yield break;
        }

        _hasAlreadyDrawn = true;
        _spellParticleSystem.Stop();
        RecordTheShapeOfLasso(_points);
        Invoke(nameof(ClearLasso), .5f); // delay to see the closed shape

        foreach (var hit in hits)
        {
            if (hit != null)
            {
                CardPrefab card = hit.gameObject.GetComponent<CardPrefab>();
                if (card == null || card.isLassoed) continue;
                lassoedCards.Add(card);
                card.OnLassoed();
            }
        }

        foreach (var card in lassoedCards)
        {
            card.OnActivate();
            yield return new WaitUntil(() => card == null);
        }

        lassoedCardsCount = lassoedCards.Count;

        Destroy(temp);
    }

    private void RecordTheShapeOfLasso(List<Vector2> points)
    {
        StartCoroutine(_gestureRecognizerController.RecordPoints(points, shape =>
           {
               recordedLassoShape = shape;
               OnLassoShapeRecognized?.Invoke(recordedLassoShape);
               RelicManager.ApplyEffectsForEvent<AfterLoopClosedEventWithItam>(this);
           }));
    }
}
