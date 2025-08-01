using UnityEngine;

public class EnemyBrain : MonoBehaviour, IDamageable, IDamageDealer
{
    [SerializeField] private CombatParticipantStats _stats;
    [SerializeField] private EnemyBehavior[] _enemyBehaviors;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += HandleTurnChanged;
        HandleTurnChanged(TurnManager.IsPlayerTurn);
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= HandleTurnChanged;
    }

    private void HandleTurnChanged(bool isPlayerTurn)
    {
        Color newColor = isPlayerTurn ? new Color(.5f, .5f, .5f) : Color.white;
        _spriteRenderer.color = newColor;
    }

    public void TakeAction()
    {
        int behaviorIndex = Random.Range(0, _enemyBehaviors.Length);
        EnemyBehavior selectedBehavior = _enemyBehaviors[behaviorIndex];
        selectedBehavior.TakeAction(this);
    }

    #region IDamageable Implementation
    public void TakeDamage(int damage)
    {
        _stats.SetCurrentHealth(_stats.CurrentHealth - damage);
    }

    public bool IsDead()
    {
        return _stats.CurrentHealth <= 0;
    }

    public int GetCurrentHealth()
    {
        return _stats.CurrentHealth;
    }
    #endregion

    #region IDamageDealer Implementation
    public void DealDamage(IDamageable target, int damage)
    {
        if (target == null)
        {
            Logger.LogWarning("Target is null");
            return;
        }

        target.TakeDamage(damage);
    }
    #endregion
}
