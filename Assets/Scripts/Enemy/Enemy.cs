using Deviloop;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : CombatCharacter, IPointerDownHandler
{
    public Action<EnemyAction> OnIntentionChanged;

    [Header("Visualas and Animation")]
    [SerializeField] private float _playActionDelay = 1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _parentAnimator;

    private Color _grayColor = new Color(.5f, .5f, .5f);
    private EnemyAction nextAction;
    public EnemyAction NextAction => nextAction;

    public EnemyData enemyStats => Stats as EnemyData;

    protected override void Start()
    {
        base.Start();
        PickNextAction();
    }

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += HandleTurnChanged;
        base.OnDeath += AfterDeathTrigger;
        base.OnDamageRecieved += DamageRecieved;
        GameStateManager.OnPlayerClickedThrowButton += OnPlayerClickedThrow;

        TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
    }

    private void OnDisable()
    {
        base.OnDeath -= AfterDeathTrigger;
        base.OnDamageRecieved -= DamageRecieved;
        TurnManager.OnTurnChanged -= HandleTurnChanged;
        GameStateManager.OnPlayerClickedThrowButton -= OnPlayerClickedThrow;
    }

    private void HandleTurnChanged(TurnManager.ETurnMode turnMode)
    {
        _spriteRenderer.color = Color.white;

        if (turnMode == TurnManager.ETurnMode.Player)
        {
            PickNextAction();
        }
        else
        {
            if (nextAction != null)
            {
                // TODO: move this to an enemy controller and activate them one by one
                if (!IsDead())
                    StartCoroutine(PlayNextActionWithDelay());
            }
        }
    }

    private void OnPlayerClickedThrow()
    {
        _spriteRenderer.color = _grayColor;
    }

    private void PickNextAction()
    {
        int behaviorIndex = UnityEngine.Random.Range(0, enemyStats.EnemyActions.Count);
        nextAction = enemyStats.EnemyActions[behaviorIndex];
        if (nextAction.CanBeTaken() == false)
        {
            PickNextAction();
            return;
        }
        OnIntentionChanged?.Invoke(nextAction);
    }

    private IEnumerator PlayNextActionWithDelay()
    {
        yield return new WaitForSeconds(nextAction.actionDelay);
        ApplyAllEffects(nextAction);
        OnIntentionChanged?.Invoke(null);
        nextAction.TakeAction(this, this, OnActionFinished);
    }

    private void OnActionFinished()
    {
        TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
    }

    private void AfterDeathTrigger(CombatCharacter combatCharacter)
    {
        _parentAnimator.SetTrigger("Death");
        _spriteRenderer.color = Color.gray;
        OnIntentionChanged?.Invoke(null);
    }

    private void DamageRecieved()
    {
        _animator.SetTrigger("Hit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameStateManager.IsInLassoingState) return;

        if (IsDead()) return;
        CombatTargetSelection.SetTargetAction?.Invoke(this);
    }


    public void OnDeathAnimationFinished()
    {
        // TODO: add object pooling
        Destroy(gameObject);
    }

    public override void DealDamage(IDamageable target, int damage)
    {
        base.DealDamage(target, damage);
        _animator.SetTrigger("Attack");
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        _animator.SetTrigger("GetHeal");
    }
}