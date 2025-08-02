using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // After items are thrown, these values change to allow player start drawing the lasso
    public static Action OnPlayerClickedThrowButton;
    private static bool _canPlayerDrawLasso = false;

    [SerializeField] private GameObject _gameOverPage;

    public static bool CanPlayerDrawLasso
    {
        get
        {
            return _canPlayerDrawLasso;
        }
        set
        {
            if (value)
            {
                OnPlayerClickedThrowButton?.Invoke();
            }
            _canPlayerDrawLasso = value;
        }
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        Time.timeScale = 0f;
        _gameOverPage.SetActive(true);
    }
}
