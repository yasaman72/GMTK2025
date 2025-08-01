using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
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
        Color newColor = isPlayerTurn ? new Color(.5f, .5f, .5f): Color.white;
        _spriteRenderer.color = newColor;
    }

    public void TakeAction()
    {
        int behaviorIndex = Random.Range(0, _enemyBehaviors.Length);
        EnemyBehavior selectedBehavior = _enemyBehaviors[behaviorIndex];
        selectedBehavior.TakeAction();
    }

}
