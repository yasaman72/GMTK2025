using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBrain : MonoBehaviour, IDamageable, IDamageDealer
{
    [SerializeField] private CombatParticipantStats _stats;
    [SerializeField] private EnemyBehavior[] _enemyBehaviors;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Header("Intention UI")]
    [SerializeField] private GameObject _enemyUi;
    [SerializeField] private Image _intentionIcon;
    [SerializeField] private TextMeshProUGUI _intentionText;
    [Header("HP UI")]
    [SerializeField] private TextMeshProUGUI _enemyHp;
    [SerializeField] private Slider _hpBar;
    [Header("Animation")]
    [SerializeField] private Animator _animator;

    private EnemyBehavior nextAction;
    public int MaxHealth => _stats.MaxHealth;

    private void Awake()
    {
        _stats.ResetStats();
    }

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += HandleTurnChanged;
        HandleTurnChanged(TurnManager.IsPlayerTurn);

        UpdateHpUi();
    }

    private IEnumerator Start()
    {
        _enemyUi.SetActive(false);
        yield return new WaitForSeconds(1f);
        _enemyUi.SetActive(true);
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= HandleTurnChanged;
    }

    private void HandleTurnChanged(bool isPlayerTurn)
    {
        Color newColor = isPlayerTurn ? new Color(.5f, .5f, .5f) : Color.white;
        _spriteRenderer.color = newColor;

        if (isPlayerTurn)
        {
            // pick the aciton in player's turn to show the intention
            PickNextAction();
        }
        else
        {
            // TODO: add a delay and efects and animations for the enemy action
            if (nextAction == null)
            {
                Logger.Log("Next action is null.", true);
            }
            else
            {
                nextAction.TakeAction(this);
            }
        }
    }

    private void PickNextAction()
    {
        int behaviorIndex = Random.Range(0, _enemyBehaviors.Length);
        nextAction = _enemyBehaviors[behaviorIndex];
        UpdateIntentionUI(nextAction);
    }

    private void UpdateIntentionUI(EnemyBehavior nextAction)
    {
        _intentionIcon.sprite = nextAction.icon;
        if (nextAction is EnemyBehavior_Attack attackBehavior)
        {
            _intentionText.text = attackBehavior.attackDamage.ToString();
        }
        else
        { _intentionText.text = ""; }
    }

    #region IDamageable Implementation
    public void TakeDamage(int damage)
    {
        _stats.SetCurrentHealth(_stats.CurrentHealth - damage);
        if (damage > 0)
        {
            _animator.SetTrigger("Hit");
        }

        UpdateHpUi();
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            Logger.LogWarning("Heal amount cannot be negative");
            return;
        }
        _stats.SetCurrentHealth(_stats.CurrentHealth + amount);
        UpdateHpUi();
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

    private void UpdateHpUi()
    {
        _enemyHp.text = $"{_stats.CurrentHealth}/{_stats.MaxHealth}";
        _hpBar.value = (float)_stats.CurrentHealth / _stats.MaxHealth;
    }

    public void AddShield(int amount)
    {
        throw new System.NotImplementedException();
    }
}
