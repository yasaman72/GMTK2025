using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static bool shouldLog = false;

    public static Action<bool> OnTurnChanged;
    private static bool _isPlayerTurn = true;
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
