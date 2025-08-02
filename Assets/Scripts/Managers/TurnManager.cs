using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private float _delayBeforeStar = 1.5f;

    private static bool shouldLog = false;

    public static Action<bool> OnTurnChanged;
    private static bool _isPlayerTurn = false;
    public static bool IsPlayerTurn
    {
        private set
        {
            if (_isPlayerTurn == value) return;
            OnTurnChanged?.Invoke(value);
            _isPlayerTurn = value; 
        }
        get => _isPlayerTurn;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delayBeforeStar);
        ChangeTurn(true); 
        yield return null;
    }

    public static void ChangeTurn(bool isPlayerTurn)
    {
        IsPlayerTurn = isPlayerTurn;

        if (IsPlayerTurn)
        {
            Logger.Log("Player's turn!", shouldLog);
        }
        else
        {
            Logger.Log("Enemy's turn!", shouldLog);
        }
    }
}
