using System;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    // After items are thrown, these values change to allow player start drawing the lasso
    public static Action OnPlayerClickedThrowButton;

    [SerializeField] private GameObject _gameOverPage;
    [SerializeField] private Button _throwButton;

    public static bool CanPlayerDrawLasso { get; set; }
    public static bool IsInLassoingState { get; set; }

    private void OnEnable()
    {
        _throwButton.onClick.AddListener(OnThrowClicked);
    }

    private void OnDisable()
    {
        _throwButton.onClick.RemoveListener(OnThrowClicked);
    }

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

    private void OnThrowClicked()
    {
        OnPlayerClickedThrowButton?.Invoke();
        IsInLassoingState = true;
    }
}
