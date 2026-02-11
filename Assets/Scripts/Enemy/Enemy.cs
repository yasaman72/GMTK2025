using Deviloop;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : CombatCharacter, IPointerDownHandler
{
    public Action<EnemyAction> OnIntentionChanged;

    [Header("Visualas and Animation")]
    [SerializeField] private float _playActionDelay = 1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _parentAnimator;

    private Color _grayColor = new Color(.5f, .5f, .5f);
    private EnemyAction currentAction;
    private EnemyAction previousAction;
    public EnemyAction CurrentAction => currentAction;

    public EnemyData enemyStats => Stats as EnemyData;

    protected override void Start()
    {
        base.Start();
        PickNextAction();
    }

    protected new void OnEnable()
    {
        TurnManager.OnTurnChanged += HandleTurnChanged;
        base.OnDeath += AfterDeathTrigger;
        base.OnDamageRecieved += DamageRecieved;
        CardManager.OnPlayerClickedThrowButton += OnPlayerClickedThrow;

        TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
        base.OnEnable();
    }

    protected new void OnDisable()
    {
        base.OnDeath -= AfterDeathTrigger;
        base.OnDamageRecieved -= DamageRecieved;
        TurnManager.OnTurnChanged -= HandleTurnChanged;
        CardManager.OnPlayerClickedThrowButton -= OnPlayerClickedThrow;

        base.OnDisable();
    }

    private void HandleTurnChanged(TurnManager.ETurnMode turnMode)
    {
        _spriteRenderer.color = Color.white;
    }

    private void OnPlayerClickedThrow()
    {
        _spriteRenderer.color = _grayColor;
    }

    public void PickNextAction()
    {
        if (IsDead())
            return;

        int behaviorIndex = SeededRandom.Range(0, enemyStats.EnemyActions.Count);
        previousAction = currentAction;
        EnemyAction nextAction = enemyStats.EnemyActions[behaviorIndex];
        if (nextAction.CanBeTaken(previousAction) == false)
        {
            PickNextAction();
            return;
        }
        currentAction = nextAction;
        OnIntentionChanged?.Invoke(currentAction);
    }

    public async Task TakeNextActionAsync()
    {
        if (currentAction == null || IsDead())
            return;

        await Awaitable.WaitForSecondsAsync(currentAction.actionDelay.Value);

        ApplyAllEffects(currentAction);
        OnIntentionChanged?.Invoke(null);

        await ExecuteActionAsync();
    }

    private Task ExecuteActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();

        currentAction.TakeAction(this, this, () =>
        {
            tcs.SetResult(true);
        });

        return tcs.Task;
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

    public override void DealDamage(IDamageable target, int damage, AttackType type)
    {
        base.DealDamage(target, damage, type);
        _animator.SetTrigger("Attack");
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        _animator.SetTrigger("GetHeal");
    }
}