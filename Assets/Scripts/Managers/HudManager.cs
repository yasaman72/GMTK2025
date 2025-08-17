using Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : CustomMonoBehavior
{
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private Image _turnBg;
    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private TextMeshProUGUI _playerHpText;
    [SerializeField] private TextMeshProUGUI _playerShield;
    [SerializeField] private Image _bgImage;
    [SerializeField] private GameObject _pauseMenu;
    [Space]
    // TODO: this is a dirty fix
    [SerializeField] private DeckView _deckView;
    [SerializeField] private RewardView _rewardView;

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += UpdateTurnText;
        _pauseMenu.SetActive(false);
        _deckView.Initialize();
        _rewardView.Initialize();
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= UpdateTurnText;
    }

    private void Start()
    {
        InitializeHud();
    }
    private void InitializeHud()
    {
        UpdateTurnText(TurnManager.TurnMode);
    }

    private void UpdateTurnText(TurnManager.ETurnMode turnMode)
    {
        bool isPlayerTurn = turnMode == TurnManager.ETurnMode.Player;
        _turnText.text = isPlayerTurn ? "Your Turn!" : "Enemy Turn!";
        _turnBg.color = isPlayerTurn ? Color.black : Color.red;
        Color newColor = isPlayerTurn ? new Color(.5f, .5f, .5f) : Color.white;
        _bgImage.color = newColor;
    }

    public void OnPause()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }

    // TODO: find a single solution for opening any deck
    public void OpenDrawDeck()
    {
        DeckView.OpenDeck?.Invoke(CardManager.DrawDeck);
    }

    public void OpenDiscardDeck()
    {
        DeckView.OpenDeck?.Invoke(CardManager.DiscardDeck);
    }
}
