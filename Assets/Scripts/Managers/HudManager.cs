using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private TextMeshProUGUI _playerHpText;
    [SerializeField] private TextMeshProUGUI _playerShield;
    [SerializeField] private Image _bgImage;

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += UpdateTurnText;
        PlayerManager.OnPlayerHPChanged += UpdatePlayerHpVisuals;
        PlayerManager.OnPlayerShieldChanged += UpdatePlayerShieldVisuals;
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= UpdateTurnText;
        PlayerManager.OnPlayerHPChanged -= UpdatePlayerHpVisuals;
        PlayerManager.OnPlayerShieldChanged -= UpdatePlayerShieldVisuals;
    }

    private void UpdatePlayerShieldVisuals(int shieldAmount)
    {
        _playerShield.text = $"{shieldAmount}";
    }

    private void Start()
    {
        InitializeHud();
    }
    private void InitializeHud()
    {
        UpdateTurnText(TurnManager.IsPlayerTurn);
        UpdatePlayerHpVisuals(
            PlayerManager.PlayerDamageableInstance.GetCurrentHealth(),
            PlayerManager.PlayerDamageableInstance.MaxHealth);
    }

    private void UpdateTurnText(bool isPlayerTurn)
    {
        _turnText.text = isPlayerTurn ? "Your Turn!" : "Enemy Turn!";

        Color newColor = isPlayerTurn ? new Color(.5f, .5f, .5f) : Color.white;
        _bgImage.color = newColor;
    }

    private void UpdatePlayerHpVisuals(int currentHp, int maxHp)
    {
        _playerHpText.text = $"{currentHp} / {maxHp}";
        _playerHpSlider.value = (float)currentHp / maxHp;
    }
}
