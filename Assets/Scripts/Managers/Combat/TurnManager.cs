using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private float _delayBeforeStart = 1.5f;

    public enum ETurnMode
    {
        Player,
        Enemy,
        NONE
    }

    private static bool shouldLog = false;

    public static Action<ETurnMode> OnTurnChanged;
    private static ETurnMode _turnMode = ETurnMode.Player;
    public static ETurnMode TurnMode
    {
        private set
        {
            if (_turnMode == value) return;
            OnTurnChanged?.Invoke(value);
            _turnMode = value;
        }
        get => _turnMode;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delayBeforeStart);
        ChangeTurn(ETurnMode.Player);
        yield return null;
    }

    public static void ChangeTurn(ETurnMode newTurnMode)
    {
        TurnMode = newTurnMode;

        switch (TurnMode)
        {
            case ETurnMode.Player:
                Logger.Log("Player's turn!", shouldLog);
                break;
            case ETurnMode.Enemy:
                Logger.Log("Enemy's turn!", shouldLog);
                break;
            case ETurnMode.NONE:
                Logger.Log("No turn mode set!", shouldLog);
                break;
            default:
                Logger.LogError("Unknown turn mode: " + newTurnMode);
                break;
        }
    }
}
