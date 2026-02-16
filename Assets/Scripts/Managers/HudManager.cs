using Deviloop;
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
    [SerializeField] private SpriteRenderer _bgImage;
    [SerializeField] private PauseMenuManager _pauseMenu;
    [SerializeField] private TextMeshProUGUI _buildNumber;
    [Space]
    // TODO: this is a dirty fix
    [SerializeField] private DeckView _deckView;
    [SerializeField] private RewardView _rewardView;

    private Color _grayColor = new Color(.5f, .5f, .5f);

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += UpdateTurnText;
        CardManager.OnPlayerClickedThrowButton += OnPlayerClickedThrow;

        _pauseMenu.gameObject.SetActive(false);
        _deckView.Initialize();
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= UpdateTurnText;
        CardManager.OnPlayerClickedThrowButton -= OnPlayerClickedThrow;
        _rewardView.OnReset();
        _deckView.OnReset();
    }


    private void Start()
    {
        _buildNumber.text = "Build: " + Application.version;
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
        _bgImage.color = Color.white;
    }

    private void OnPlayerClickedThrow()
    {
        _bgImage.color = _grayColor;
    }

    public void OnPause()
    {
        UIViewsManager.Instance.OpenPage(_pauseMenu);
    }

    // TODO: find a single solution for opening any deck
    public void OpenDrawDeck()
    {
        DeckView.OpenDeck?.Invoke(CardManager.DrawDeck, null);
    }

    public void OpenDiscardDeck()
    {
        DeckView.OpenDeck?.Invoke(CardManager.DiscardDeck, null);
    }
}
