using System.Collections;
using System.Collections.Generic;
using Cards.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

namespace Cards
{
    public class CardManager : MonoBehaviour
    {
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

        [Header("Throwing Settings")]
        public int cardsToThrowPerTurn = 5;
        public Transform throwOrigin; // Bottom of screen
        public float delayBetweenThrows = 0.2f;
        public Vector2 throwRange = new Vector2(-5f, 5f);
        public Vector2 throwForceRange = new Vector2(5f, 15f);
        public Vector2 throwAngleRange = new Vector2(60f, 120f); // Degrees
        public Vector2 torqueRange = new Vector2(-5, 5);
        [Header("Audio Settings")]
        public AudioClip throwSound;
        public AudioClip throwStartSound;

        [Header("UI Elements")]
        public Button throwButton;
        public TextMeshProUGUI deckCountText;
        public TextMeshProUGUI discardDeckCountText;
        public Transform deckDisplayParent; // For showing remaining cards
        public float enemyTurnDuration = 2f;

        // Runtime variables
        private List<GameObject> thrownCards = new();

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            // Create runtime copy of deck
            _drawDeck = Instantiate(baseDeck);
            _drawDeck.InitializeDeck();

            _discardDeck = Instantiate(baseDeck);
            _discardDeck.InitializeDeck();
            _discardDeck.RemoveAllCards();

            // Setup UI
            throwButton.onClick.AddListener(OnThrowButtonClicked);
            UpdateUI();
        }

        void UpdateUI()
        {
            // Update deck count
            int totalCards = _drawDeck.GetTotalCardCount();
            deckCountText.text = totalCards.ToString();
            discardDeckCountText.text = _discardDeck.GetTotalCardCount().ToString();
        }

        public void OnThrowButtonClicked()
        {
            if (!TurnManager.IsPlayerTurn) return;

            StartCoroutine(ThrowCardsSequence());
        }

        IEnumerator ThrowCardsSequence()
        {
            throwButton.gameObject.SetActive(false);

            List<BaseCard> cardsToThrow = SelectCardsToThrow();

            ClearThrownCards();

            AudioManager.OnPlaySoundEffct?.Invoke(throwStartSound);

            // Throw cards with slight delays
            for (int i = 0; i < cardsToThrow.Count; i++)
            {
                ThrowCard(cardsToThrow[i]);
                AudioManager.OnPlaySoundEffct?.Invoke(throwSound);
                yield return new WaitForSeconds(delayBetweenThrows); // Small delay between throws
            }

            GameStateManager.CanPlayerDrawLasso = true;
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

            // Switch to enemy turn
            SetPlayerTurn(false);
            // TODO: switch to players turn after all items are activated or all items are out of the view
            yield return new WaitForSeconds(enemyTurnDuration);

            // Back to player turn
            SetPlayerTurn(true);

            if (_drawDeck.GetTotalCardCount() <= 0)
                ReturnCardsToDrawDeck();
        }

        List<BaseCard> SelectCardsToThrow()
        {
            List<BaseCard> allCards = _drawDeck.GetAllCardsAsList();
            List<BaseCard> selectedCards = new List<BaseCard>();

            for (int i = 0; i < cardsToThrowPerTurn; i++)
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

            return selectedCards;
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
            foreach (var card in thrownCards)
            {
                if (card != null)
                    Destroy(card);
            }
            thrownCards.Clear();
        }

        public void AddCardToDiscard(CardEntry card)
        {
            for (int i = 0; i < card.quantity; i++)
            {
                _discardDeck.AddCard(card.cardType);
            }

            UpdateUI();
        }

        // TODO: move to a proper class
        void SetPlayerTurn(bool playerTurn)
        {
            TurnManager.ChangeTurn(playerTurn);
            throwButton.gameObject.SetActive(playerTurn);
        }
    }
}