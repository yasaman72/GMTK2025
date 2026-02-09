using Cards;
using Deviloop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class PlayerLassoManager : MonoBehaviour
{
    public static Action<LassoShape> OnLassoShapeRecognized;
    public delegate void LassoSizeChanged(int maxSize, int currentSize);
    public static LassoSizeChanged OnLassoSizeChanged;
    public static Action OnLoopClosed;


    [SerializeField] private bool shouldLog = true;
    [SerializeField] private bool shouldPauseOnLoop = true;
    [Space]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private ContactFilter2D _itemsFilter;
    [SerializeField] private float _pointSpacing = 0.1f;
    [SerializeField] private float _pointSpacingForLoop = 0.3f;
    [SerializeField] private float _loopCloseThreshold = 0.5f;
    [SerializeField] private float _minDistanceToDrawFinalPoint = 0.2f;
    [SerializeField] private Gradient _defaultColor;
    [SerializeField] private Gradient _nearCloseColor;
    [Space]
    [SerializeField] private float _slowMotionTimeScale = 0.2f;
    [SerializeField] private int _lassoLength = 40;
    [SerializeField] private int _minPOintForLoop = 10;
    [SerializeField] private int _maxAllowedItems = 3;
    [SerializeField] private ParticleSystem _spellParticleSystem;
    [SerializeField] private GestureRecognizerController _gestureRecognizerController;

    private List<Vector2> _points = new();
    private static bool _hasAlreadyDrawn = false;
    public static bool HasAlreadyDrawn => _hasAlreadyDrawn;
    private bool _startNewLine = false;
    private Coroutine clearCoroutine;
    private bool _isResolvingALoop = false;

    // TODO: a more generic to handle values modified by relics
    private static int maxedAllowedItems;
    public static int MaxedAllowedItems
    {
        get => maxedAllowedItems;
        set
        {
            maxedAllowedItems = value;
            MaxedAllowedItemsVariable.Value = maxedAllowedItems;
        }

    }
    private static IntVariable MaxedAllowedItemsVariable;

    public static int lassoedCardsCount = 0;
    public static LassoShape recordedLassoShape = LassoShape.Unknown;

    private void Awake()
    {
        var source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        MaxedAllowedItemsVariable = source["global"]["MaxedAllowedItems"] as IntVariable;
        MaxedAllowedItemsVariable.Value = 0;
    }

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

        ImmediateLassoClear();

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
        {
            _hasAlreadyDrawn = true;
        }
        else if (turnMode == TurnManager.ETurnMode.Player)
        {
            ImmediateLassoClear();
        }
    }

    private void OnPlayerDrawTurnStart()
    {
        ImmediateLassoClear();
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
            StopCoroutine(InvertLasso());
            ImmediateLassoClear();
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

            bool isLooping = false;
            int loopEndPointIndex = IsLineLooping(out isLooping);

            if (IsLineNearOtherEnd() || isLooping)
            {
                CloseLoop(isLooping, loopEndPointIndex);
                _lineRenderer.colorGradient = _nearCloseColor;
            }
            else
            {
                _lineRenderer.colorGradient = _defaultColor;
                OnLassoSizeChanged?.Invoke(_lassoLength, _points.Count);

                // if the line is too long, remove the oldest point
                if (_points.Count > _lassoLength)
                {
                    _points.RemoveAt(0);
                    for (int i = 0; i < _points.Count; i++)
                    {
                        _lineRenderer.SetPosition(i, _points[i]);
                    }
                    _lineRenderer.positionCount = _points.Count;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _startNewLine = false;
            Time.timeScale = 1f;

            bool isLooping = false;
            int loopEndPointIndex = IsLineLooping(out isLooping);

            if (!_isResolvingALoop && (IsLineNearOtherEnd() || isLooping))
            {
                CloseLoop(isLooping, loopEndPointIndex);
            }
            else
            {
                StartCoroutine(InvertLasso());
            }
        }
    }

    private void ImmediateLassoClear(float dealy = 0)
    {
        if (clearCoroutine != null)
            StopCoroutine(clearCoroutine);
        clearCoroutine = StartCoroutine(InvertLasso(true, dealy));
    }

    private IEnumerator InvertLasso(bool immediate = false, float delay = 0)
    {
        _startNewLine = false;

        yield return new WaitForSeconds(delay);

        CancelParticles();

        if (immediate)
        {
            _lineRenderer.positionCount = 0;
            _points.Clear();
            _lineRenderer.colorGradient = _defaultColor;
            OnLassoSizeChanged?.Invoke(_lassoLength, 0);
            yield break;
        }

        // Gradually remove points from the end
        while (_lineRenderer.positionCount > 0)
        {
            _lineRenderer.positionCount--;
            OnLassoSizeChanged?.Invoke(_lassoLength, _lineRenderer.positionCount);
            yield return null;
        }

        _points.Clear();
        _lineRenderer.colorGradient = _defaultColor;
    }

    private void CancelParticles()
    {
        _spellParticleSystem.Stop();
        _spellParticleSystem.gameObject.SetActive(false);
        _spellParticleSystem.transform.position = Vector2.one * 1000;
    }

    private bool IsLineNearOtherEnd()
    {
        return _points.Count > 10 && Vector2.Distance(_points[^1], _points[0]) < _loopCloseThreshold;
    }

    private int IsLineLooping(out bool isLooping)
    {
        if (_points.Count < _pointSpacingForLoop)
        {
            isLooping = false;
            return -1; // not enough points to form a loop
        }
        Vector2 lastPoint = _points[^1];

        // check closest points to the last point
        int closestPointIndex = -1;
        for (int i = 0; i < _points.Count - _minPOintForLoop; i++)
        {
            if (Vector2.Distance(lastPoint, _points[i]) < (_loopCloseThreshold))
            {
                if (closestPointIndex == -1)
                {
                    closestPointIndex = i;
                    continue;
                }
                if (Vector2.Distance(lastPoint, _points[i]) < Vector2.Distance(lastPoint, _points[closestPointIndex]))
                {
                    closestPointIndex = i;
                }
            }
            else
            {
                Logger.Log($"Point {i} is too far from the last point to form a loop. Distance: {Vector2.Distance(lastPoint, _points[i])}", shouldLog);
            }
        }

        if (closestPointIndex == -1)
        {
            isLooping = false;
            return -1;
        }
        else
        {
            isLooping = true;
            return closestPointIndex;
        }
    }

    void CloseLoop(bool isLooping, int closestPointToLoopEndIndex)
    {
        _isResolvingALoop = true;
        Time.timeScale = 1f;

        Vector2 newPoint = isLooping ? _points[closestPointToLoopEndIndex] : _points[0];
        // if points are too close, don't add the point
        if (Vector2.Distance(newPoint, _points[^1]) > _minDistanceToDrawFinalPoint)
        {
            _points.Add(newPoint);
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPosition(_points.Count - 1, newPoint);
            Logger.Log($"Loop closed! new point: {newPoint}", shouldLog);

#if UNITY_EDITOR
            EditorApplication.isPaused = shouldPauseOnLoop ? true : false;
#endif
        }

        // only send the points that are inside the loop to be detected
        var loopPoints = new List<Vector2>(_points);
        if (isLooping)
        {
            loopPoints = new List<Vector2>();
            for (int i = closestPointToLoopEndIndex; i < _points.Count; i++)
            {
                loopPoints.Add(_points[i]);
            }
        }

        StartCoroutine(DetectInsidePoints(loopPoints));
    }

    private IEnumerator DetectInsidePoints(List<Vector2> loopPoints)
    {
        yield return null;
        GameObject temp = new GameObject("TempCollider");
        PolygonCollider2D poly = temp.AddComponent<PolygonCollider2D>();
        poly.isTrigger = true;
        poly.points = loopPoints.ToArray();
        List<Collider2D> hits = new List<Collider2D>();
        Physics2D.OverlapCollider(poly, _itemsFilter, hits);

#if UNITY_EDITOR
        EditorApplication.isPaused = shouldPauseOnLoop ? true : false;
#endif

        List<CardPrefab> lassoedCards = new List<CardPrefab>();

        if (hits.Count <= 0 || hits.Count > maxedAllowedItems)
        {
            if (hits.Count > maxedAllowedItems)
                MessageController.OnDisplayMessage?.Invoke($"Max allowed items is {maxedAllowedItems}. Try again!", 2);

            Time.timeScale = 1f;
            Destroy(temp);
            StartCoroutine(InvertLasso());
            yield break;
        }

        _hasAlreadyDrawn = true;
        _spellParticleSystem.Stop();
        ImmediateLassoClear(0.5f);

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

        lassoedCardsCount = lassoedCards.Count;
        RecordTheShapeOfLasso(_points);

        foreach (var card in lassoedCards)
        {
            card.OnActivate();
            yield return new WaitUntil(() => card == null);
        }

        Destroy(temp);
        _isResolvingALoop = false;
        OnLoopClosed?.Invoke();
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
