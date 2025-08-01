using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private TextMeshProUGUI _playerHpText;

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += UpdateTurnText;
        PlayerManager.OnPlayerHPChanged += UpdatePlayerHpVisuals;

        InitializeHud();
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= UpdateTurnText;
        PlayerManager.OnPlayerHPChanged -= UpdatePlayerHpVisuals;
    }

    private void InitializeHud()
    {
        _turnText.text = TurnManager.IsPlayerTurn ? "Your Turn!" : "Enemy Turn!";
    }

    private void UpdateTurnText(bool isPlayerTurn)
    {
        _turnText.text = isPlayerTurn ? "Your Turn!" : "Enemy Turn!";
    }

    private void UpdatePlayerHpVisuals(int currentHp, int maxHp)
    {
        _playerHpText.text = $"{currentHp} / {maxHp}";
        _playerHpSlider.value = (float)currentHp / maxHp;
    }
}
