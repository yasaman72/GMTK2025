using Deviloop;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : CombatCharacter, IPointerDownHandler, IPoolable
{
    public Action<EnemyAction> OnIntentionChanged;

    [Header("Visualas and Animation")]
    [SerializeField] private float _playActionDelay = 1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _parentAnimator;

    private Color _grayColor = new Color(.5f, .5f, .5f);
    private EnemyAction currentAction;
    public EnemyAction CurrentAction => currentAction;

    public EnemyData enemyStats => Stats as EnemyData;
    private CancellationTokenSource _cancellationTokenSource;

    protected override void Start()
    {
        base.Start();
        PickNextAction();
    }

    public void OnSpawned()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        FullyHeal();
        RemoveAllShields();

        TurnManager.OnTurnChanged += HandleTurnChanged;
        base.OnDeath += AfterDeathTrigger;
        base.OnDamageRecieved += DamageRecieved;
        CardManager.OnPlayerClickedThrowButton += OnPlayerClickedThrow;

        PickNextAction();
    }

    public void OnDespawned()
    {
        base.OnDeath -= AfterDeathTrigger;
        base.OnDamageRecieved -= DamageRecieved;
        TurnManager.OnTurnChanged -= HandleTurnChanged;
        CardManager.OnPlayerClickedThrowButton -= OnPlayerClickedThrow;

        _cancellationTokenSource?.Cancel();
        RemoveAllEffects();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }

    private void HandleTurnChanged(TurnManager.ETurnMode turnMode)
    {
        _spriteRenderer.color = Color.white;
    }

    private void OnPlayerClickedThrow()
    {
        if (_spriteRenderer != null)
            _spriteRenderer.color = _grayColor;
    }

    public void PickNextAction()
    {
        if (IsDead())
            return;

        int behaviorIndex = SeededRandom.Range(0, enemyStats.EnemyActions.Count);
        EnemyAction previousAction = currentAction;
        currentAction = enemyStats.GetNextAction(previousAction);
        OnIntentionChanged?.Invoke(currentAction);
    }

    public async Task TakeNextActionAsync()
    {
        if (currentAction == null || IsDead())
            return;

        await Awaitable.WaitForSecondsAsync(currentAction.actionDelay.Value, _cancellationTokenSource.Token);

        ApplyAllEffects(currentAction);

        if (IsDead())
            return;

        OnIntentionChanged?.Invoke(null);
        await ExecuteActionAsync();
    }

    private Task ExecuteActionAsync()
    {
        var tcs = new TaskCompletionSource<bool>();

        currentAction.TakeAction(this, () =>
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
        if (GameStateManager.Instance.IsInLassoingState) return;

        if (IsDead()) return;
        CombatTargetSelection.SetTargetAction?.Invoke(this);
    }

    public void OnDeathAnimationFinished()
    {
        PoolManager.Instance.GetPool<Enemy>(this).ReturnToPool(this);
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