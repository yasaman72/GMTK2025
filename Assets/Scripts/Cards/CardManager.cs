using Cards.ScriptableObjects;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Cards
{
    public class CardManager : MonoBehaviour
    {
        // After items are thrown, these values change to allow player start drawing the lasso
        public static Action OnPlayerClickedThrowButton;
        public static Action<BaseCard, int> AddCardToDeckAction;
        public delegate void RemoveCardFromDeck(BaseCard card, bool removeFromDiscard);
        public static RemoveCardFromDeck RemoveCardFromDeckAction;
        public static Action<CardEntry> AddCardToHandAction;
        public static Action ReturnAllCardsToHand;

        [SerializeField] private bool shouldLog = true;

        [Header("Deck Configuration")]
        public CardDeck baseDeck;
        public static CardDeck _drawDeck;
        public static CardDeck DrawDeck
        {
            get => _drawDeck;
        }
        private static CardDeck _discardDeck;
        public static CardDeck DiscardDeck
        {
            get => _discardDeck;
        }
        public CardDeck _extraHandCards;

        [Header("Throwing Settings")]
        private static int _cardsToThrowPerTurn = 5;
        public static int CardsToThrowPerTurn
        {
            set
            {
                _cardsToThrowPerTurn = Mathf.Max(1, value);
                CombatRoundCounterVariable.Value = CardsToThrowPerTurn;
            }
            get => _cardsToThrowPerTurn;
        }
        public static bool ShouldThrowAtOnce = false;
        public Transform throwOrigin; // Bottom of screen
        public float delayBetweenThrows = 0.2f;
        public Vector2 throwRange = new Vector2(-5f, 5f);
        public Vector2 throwForceRange = new Vector2(5f, 15f);
        public Vector2 throwAngleRange = new Vector2(60f, 120f); // Degrees
        public Vector2 torqueRange = new Vector2(-5, 5);

        [Header("Audio Settings")]
        public EventReference throwSound;
        public EventReference throwStartSound;

        [Header("UI Elements")]
        public Button throwButton;
        public TextMeshProUGUI deckCountText;
        public TextMeshProUGUI discardDeckCountText;
        public Transform deckDisplayParent;
        public float enemyTurnDuration = 2f;

        // Runtime variables
        private List<GameObject> thrownCards = new();

        // TODO: move all the global localized variables to another class
        private static IntVariable CombatRoundCounterVariable;


        private void Awake()
        {
            var source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
            CombatRoundCounterVariable = source["global"]["CurrentThrownItemsCount"] as IntVariable;
            CombatRoundCounterVariable.Value = CardsToThrowPerTurn;
        }
        void Start()
        {
            Initialize();
        }

        private void Update()
        {
            // TODO: replace with new input system when implemented
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnThrowButtonClicked();
            }
        }

        private void OnEnable()
        {
            TurnManager.OnTurnChanged += HandleTurnChanged;
            AddCardToDeckAction += AddCardToDeck;
            AddCardToHandAction += AddCardToHand;
            ReturnAllCardsToHand += ReturnCardsToDrawDeck;
            RemoveCardFromDeckAction += RemoveCard;
            CombatManager.OnAfterAllEnemiesDefeated += FinishEncounter;

        }

        private void OnDisable()
        {
            TurnManager.OnTurnChanged -= HandleTurnChanged;
            AddCardToDeckAction -= AddCardToDeck;
            AddCardToHandAction -= AddCardToHand;
            ReturnAllCardsToHand -= ReturnCardsToDrawDeck;
            RemoveCardFromDeckAction -= RemoveCard;
            CombatManager.OnAfterAllEnemiesDefeated -= FinishEncounter;
        }

        private void FinishEncounter()
        {
            throwButton.gameObject.SetActive(false);
        }

        void Initialize()
        {
            // TODO: review CardDeck class
            _drawDeck = Instantiate(baseDeck);
            _drawDeck.InitializeDeck();

            _discardDeck = Instantiate(baseDeck);
            _discardDeck.InitializeDeck();
            _discardDeck.RemoveAllCards();

            _extraHandCards = Instantiate(baseDeck);
            _extraHandCards.InitializeDeck();
            _extraHandCards.RemoveAllCards();

            // Setup UI
            throwButton.onClick.AddListener(OnThrowButtonClicked);
            UpdateUI();

            throwButton.gameObject.SetActive(true/*TurnManager.TurnMode == TurnManager.ETurnMode.Player*/);
        }

        void UpdateUI()
        {
            // Update deck count
            int totalCards = _drawDeck.GetTotalCardCount();
            deckCountText.text = totalCards.ToString();
            discardDeckCountText.text = _discardDeck.GetTotalCardCount().ToString();
        }

        private void HandleTurnChanged(TurnManager.ETurnMode turnMode)
        {
            if (turnMode == TurnManager.ETurnMode.Player && !Player.PlayerCombatCharacter.IsDead())
            {
                throwButton.gameObject.SetActive(true);
                UpdateUI();
            }
            else
            {
                throwButton.gameObject.SetActive(false);
            }
        }

        public void OnThrowButtonClicked()
        {
            if (GameStateManager.IsInLassoingState) return;
            if (TurnManager.TurnMode != TurnManager.ETurnMode.Player) return;

            StartCoroutine(ThrowCardsSequence());
            GameStateManager.IsInLassoingState = true;
            OnPlayerClickedThrowButton?.Invoke();
        }

        IEnumerator ThrowCardsSequence()
        {
            throwButton.gameObject.SetActive(false);

            List<BaseCard> cardsToThrow = SelectCardsToThrow();

            AudioManager.PlayAudioOneShot?.Invoke(throwStartSound);

            cardsToThrow = ShuffleCards(cardsToThrow);

            GameStateManager.CanPlayerDrawLasso = true;
            // Throw cards with slight delays
            for (int i = 0; i < cardsToThrow.Count; i++)
            {
                ThrowCard(cardsToThrow[i]);
                AudioManager.PlayAudioOneShot?.Invoke(throwSound);
                yield return new WaitForSeconds(ShouldThrowAtOnce ? 0 : delayBetweenThrows);
            }

            while (thrownCards.Count > 0)
            {
                // Wait until all thrown cards are out of view or destroyed
                yield return new WaitForEndOfFrame();
                if (thrownCards.TrueForAll(card => card == null || !card.activeInHierarchy))
                {
                    break;
                }
            }
            GameStateManager.CanPlayerDrawLasso = false;

            ClearThrownCards();
            AfterPlayerTurnEnd();
        }

        List<BaseCard> SelectCardsToThrow()
        {
            List<BaseCard> allCards = _drawDeck.GetAllCardsAsList();
            List<BaseCard> selectedCards = new List<BaseCard>();

            for (int i = 0; i < CardsToThrowPerTurn; i++)
            {
                if (allCards.Count <= 0)
                {
                    ReturnCardsToDrawDeck();
                    allCards = _drawDeck.GetAllCardsAsList();
                }

                int randomIndex = Random.Range(0, allCards.Count);
                selectedCards.Add(allCards[randomIndex]);
                _discardDeck.AddCard(allCards[randomIndex]);
                allCards.RemoveAt(randomIndex);
            }

            // Remove thrown cards from deck
            foreach (var card in selectedCards)
            {
                _drawDeck.RemoveCard(card);
            }
            UpdateUI();

            // always add combo card
            foreach (var card in _extraHandCards.GetAllCardsAsList())
            {
                selectedCards.Add(card);
            }

            return selectedCards;
        }

        List<BaseCard> ShuffleCards(List<BaseCard> originalCards)
        {
            // randomize cards order before throwing
            List<BaseCard> shuffledCards = new List<BaseCard>();
            int cardsCount = originalCards.Count;
            while (shuffledCards.Count < cardsCount)
            {
                int randomIndex = Random.Range(0, originalCards.Count);
                shuffledCards.Add(originalCards[randomIndex]);
                originalCards.RemoveAt(randomIndex);
            }

            return shuffledCards;
        }

        private void ReturnCardsToDrawDeck()
        {
            List<BaseCard> discardCards = _discardDeck.GetAllCardsAsList();
            foreach (var card in discardCards)
            {
                _drawDeck.AddCard(card);
            }
            _discardDeck.RemoveAllCards();
            UpdateUI();
        }

        void ThrowCard(BaseCard card)
        {
            if (card.cardPrefab == null)
            {
                Logger.LogWarning($"No prefab assigned for {card.cardName}");
                return;
            }

            Vector2 origin = (Vector2)throwOrigin.position + (Vector2.right * Random.Range(throwRange.x, throwRange.y));
            GameObject thrownCard = Instantiate(card.cardPrefab, origin, Quaternion.identity);
            thrownCards.Add(thrownCard);

            // Add rigidbody if not present
            Rigidbody2D rb = thrownCard.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = thrownCard.AddComponent<Rigidbody2D>();
            }

            // Calculate throw force
            float angle = Random.Range(throwAngleRange.x, throwAngleRange.y) * Mathf.Deg2Rad;
            float force = Random.Range(throwForceRange.x, throwForceRange.y);
            float torque = Random.Range(torqueRange.x, torqueRange.y);

            Vector3 throwDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            rb.AddForce(throwDirection * force, ForceMode2D.Impulse);
            rb.AddTorque(torque, ForceMode2D.Impulse);

            Logger.Log($"Threw {card.cardName}", shouldLog);
        }

        void ClearThrownCards()
        {
            if (gameObject == null) return;

            foreach (var card in thrownCards)
            {
                if (card != null)
                    Destroy(card);
            }
            thrownCards.Clear();
            if (_extraHandCards != null)
                _extraHandCards.RemoveAllCards();
        }

        public void AddCardToHand(CardEntry card)
        {
            for (int i = 0; i < card.Quantity; i++)
            {
                _extraHandCards.AddCard(card.Card);
            }
        }

        private void AddCardToDeck(BaseCard card, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                _drawDeck.AddCard(card);
            }

            UpdateUI();
        }

        private void RemoveCard(BaseCard card, bool removeFromDiscardDeck = false)
        {
            if (removeFromDiscardDeck)
                _discardDeck.RemoveCard(card);
            else
                _drawDeck.RemoveCard(card);

            UpdateUI();
        }

        private void AfterPlayerTurnEnd()
        {
            throwButton.gameObject.SetActive(false);
            TurnManager.ChangeTurn(TurnManager.ETurnMode.Enemy);
            GameStateManager.IsInLassoingState = false;

            if (_drawDeck.GetTotalCardCount() <= 0)
                ReturnCardsToDrawDeck();
        }
    }
}