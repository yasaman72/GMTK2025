using System;
using System.Collections;
using UnityEngine;

public class Enemy : CombatCharacter
{
    public Action<EnemyAction> OnIntentionChanged;

    [Header("Visualas and Animation")]
    [SerializeField] private float _playActionDelay;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _parentAnimator;

    private EnemyAction nextAction;

    private EnemyStats enemyStats => Stats as EnemyStats;

    protected override void Start()
    {
        base.Start();
        PickNextAction();
    }

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += HandleTurnChanged;
        base.OnDeath += AfterDeathTrigger;
        TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
    }

    private void OnDisable()
    {
        base.OnDeath -= AfterDeathTrigger;
        TurnManager.OnTurnChanged -= HandleTurnChanged;
    }

    private void HandleTurnChanged(TurnManager.ETurnMode turnMode)
    {
        Color newColor = turnMode == TurnManager.ETurnMode.Player ? new Color(.5f, .5f, .5f) : Color.white;
        _spriteRenderer.color = newColor;

        if (turnMode == TurnManager.ETurnMode.Player)
        {
            PickNextAction();
        }
        else
        {
            if (nextAction != null)
            {
                if (!IsDead())
                    nextAction.TakeAction(this, this, OnActionFinished);
            }
        }
    }

    private void PickNextAction()
    {
        int behaviorIndex = UnityEngine.Random.Range(0, enemyStats.EnemyActions.Count);
        nextAction = enemyStats.EnemyActions[behaviorIndex];
        OnIntentionChanged?.Invoke(nextAction);
    }

    private void OnActionFinished()
    {
        TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
    }

    private void AfterDeathTrigger()
    {
        _parentAnimator.SetTrigger("Death");
        _spriteRenderer.color = Color.gray;
        OnIntentionChanged?.Invoke(null);
    }
}