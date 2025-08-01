using UnityEngine;

[CreateAssetMenu(fileName = "Stats_Player_Enemy_", menuName = "ScriptableObjects/Combat/CombatParticipantStats", order = 1)]
public class CombatParticipantStats : ScriptableObject
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth = 100;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public void SetCurrentHealth(int health)
    {
        _currentHealth = Mathf.Clamp(health, 0, _maxHealth);
    }
}
