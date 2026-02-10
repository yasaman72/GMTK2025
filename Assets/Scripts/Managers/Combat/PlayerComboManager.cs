using Deviloop;
using System;
using TMPro;
using UnityEngine;

public class PlayerComboManager : MonoBehaviour
{
    [SerializeField] private bool _isEnabled;
    [SerializeField] private CardEntry _comboCard;
    [SerializeField] private TextMeshProUGUI _playerComboCounter;
    [SerializeField] private int _minComboToShow = 2;
    [SerializeField] private GameObject _playerComboText;

    public static Action OnPlayerComboBreak;
    private static Action UpdatePlayerComboUIAction;

    private static int _playerCombo = 0;


    private void OnEnable()
    {
        _playerComboCounter.text = "";
        _playerComboText.SetActive(false);

        if (!_isEnabled)
        {
            this.enabled = false;
            return;
        }

        OnPlayerComboBreak += OnComboBreak;
        UpdatePlayerComboUIAction += UpdatePlayerComboUI;
        TurnManager.OnTurnChanged += HandleTurnChanged;

    }

    private void OnDisable()
    {
        OnPlayerComboBreak -= OnComboBreak;
        UpdatePlayerComboUIAction -= UpdatePlayerComboUI;
        TurnManager.OnTurnChanged -= HandleTurnChanged;
    }

    private void HandleTurnChanged(TurnManager.ETurnMode mode)
    {
        CardManager.AddCardToHandAction?.Invoke(_comboCard);
    }

    private void OnComboBreak()
    {
        _playerCombo = 0;
        UpdatePlayerComboUIAction?.Invoke();
    }

    public static void OnCombo()
    {
        _playerCombo++;
        UpdatePlayerComboUIAction?.Invoke();
    }

    private void UpdatePlayerComboUI()
    {
        string text = "";
        bool showCombo = (_playerCombo >= _minComboToShow);
        _playerComboText.SetActive(showCombo);

        if (showCombo)
        {
            text = _playerCombo.ToString() + 'X';

        }
        _playerComboCounter.text = text;
    }
}

