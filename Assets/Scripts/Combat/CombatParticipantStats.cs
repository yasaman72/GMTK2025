using UnityEngine;

[CreateAssetMenu(fileName = "Stats_Player_Enemy_", menuName = "ScriptableObjects/Combat/CombatParticipantStats", order = 1)]
public class CombatParticipantStats : ScriptableObject
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth = 100;
    [SerializeField] private int _currentShield = 0;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int CurrentShield => _currentShield;
    public bool SetCurrentHealth(int health)
    {
        _currentHealth = Mathf.Clamp(health, 0, _maxHealth);
        return _currentHealth <= 0;
    }

    public void AddShield(int shield)
    {
        _currentShield += shield;
    }

    public int DamageShield(int damageAmount)
    {
        if (_currentShield >= damageAmount)
        {
            _currentShield -= damageAmount;
            return 0; // No damage taken, shield absorbed all
        }
        else
        {
            int remainingDamage = damageAmount - _currentShield;
            _currentShield = 0;
            return remainingDamage; // Return the damage that needs to be applied to health
        }
    }


    public void ResetStats()
    {
        _currentHealth = _maxHealth;
        _currentShield = 0;
    }
}
