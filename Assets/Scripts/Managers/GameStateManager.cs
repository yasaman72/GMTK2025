using System;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPage;

    public static bool CanPlayerDrawLasso { get; set; }
    public static bool IsInLassoingState { get; set; }

    private void Start()
    {
        Player.PlayerCombatCharacter.OnDeath += HandlePlayerDeath;
    }

    private void OnDestroy()
    {
        Player.PlayerCombatCharacter.OnDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath(CombatCharacter combatCharacter)
    {
        Time.timeScale = 0f;
        _gameOverPage.SetActive(true);
    }
}
