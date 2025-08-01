using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
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
            Debug.Log("Player's turn!");
        }
        else
        {
            Debug.Log("Enemy's turn!");
        }
    }
}
