using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBrain : MonoBehaviour, IDamageable, IDamageDealer
{
    public Action OnEnemyDeath;

    [SerializeField] public EnemyStats stats;
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
    [SerializeField] private Animator _parentAnimator;
    [Header("Audio")]
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _deathSound;

    private EnemyBehavior nextAction;
    public int MaxHealth => stats.MaxHealth;

    private void Awake()
    {
        stats.ResetStats();
    }
    private void OnEnable()
    {
        TurnManager.OnTurnChanged += HandleTurnChanged;
        TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);

        PickNextAction();
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
        int behaviorIndex = UnityEngine.Random.Range(0, _enemyBehaviors.Length);
        nextAction = _enemyBehaviors[behaviorIndex];
        UpdateIntentionUI(nextAction);
    }

    private void OnActionFinished()
    {
        TurnManager.ChangeTurn(TurnManager.ETurnMode.Player);
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
        if (IsDead()) return;

        stats.SetCurrentHealth(stats.CurrentHealth - damage);
        if (damage > 0)
        {
            _animator.SetTrigger("Hit");
            AudioManager.OnPlaySoundEffct?.Invoke(_hitSound);
        }

        if (IsDead())
        {
            _animator.SetBool("IsDead", true);
            _parentAnimator.SetTrigger("Death");
            AudioManager.OnPlaySoundEffct?.Invoke(_deathSound);
            OnEnemyDeath?.Invoke();
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
        stats.SetCurrentHealth(stats.CurrentHealth + amount);
        UpdateHpUi();
    }

    public bool IsDead()
    {
        return stats.CurrentHealth <= 0;
    }

    public int GetCurrentHealth()
    {
        return stats.CurrentHealth;
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
        _enemyHp.text = $"{stats.CurrentHealth}/{stats.MaxHealth}";
        _hpBar.value = (float)stats.CurrentHealth / stats.MaxHealth;
    }

    public void AddShield(int amount)
    {
        throw new System.NotImplementedException();
    }
}
