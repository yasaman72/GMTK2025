using System;
using TMPro;
using UnityEngine;

public class PlayerComboManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerComboCounter;
    [SerializeField] private int _minComboToShow = 2;
    [SerializeField] private GameObject _playerComboText;

    public static Action OnPlayerComboBreak;
    private static Action UpdatePlayerComboUIAction;

    private static int _playerCombo = 0;


    private void OnEnable()
    {
        OnPlayerComboBreak += OnComboBreak;
        UpdatePlayerComboUIAction += UpdatePlayerComboUIA;

        _playerComboCounter.text = "";
        _playerComboText.SetActive(false);
    }

    private void OnDisable()
    {
        OnPlayerComboBreak -= OnComboBreak;
        UpdatePlayerComboUIAction -= UpdatePlayerComboUIA;
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

    private void UpdatePlayerComboUIA()
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

