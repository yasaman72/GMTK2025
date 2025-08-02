using System.Collections;
using System.Collections.Generic;
using Cards.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

namespace Cards
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private bool shouldLog = true;

        [Header("Deck Configuration")]
        public CardDeck baseDeck; // Assign in inspector
        private CardDeck runtimeDeck; // Copy for gameplay

        [Header("Throwing Settings")]
        public int cardsToThrowPerTurn = 5;
        public Transform throwOrigin; // Bottom of screen
        public Vector2 throwRange = new Vector2(-5f, 5f);
        public Vector2 throwForceRange = new Vector2(5f, 15f);
        public Vector2 throwAngleRange = new Vector2(60f, 120f); // Degrees
        public Vector2 torqueRange = new Vector2(-5, 5);

        [Header("UI Elements")]
        public Button throwButton;
        public TextMeshProUGUI deckCountText;
        public Transform deckDisplayParent; // For showing remaining cards
        [SerializeField] GameObject displayObjPrefab;

        public float enemyTurnDuration = 2f;

        // Runtime variables
        private List<GameObject> thrownCards = new();
        private Dictionary<BaseCard, TextMeshProUGUI> deckDisplayTexts = new();

        private CardDeck discardPile;

        void Start()
        {
            InitializeGame();
        }

        void InitializeGame()
        {
            // Create runtime copy of deck
            runtimeDeck = Instantiate(baseDeck);
            runtimeDeck.InitializeDeck();

            discardPile = Instantiate(baseDeck);
            discardPile.InitializeDeck();
            discardPile.RemoveAllCards();

            // Setup UI
            throwButton.onClick.AddListener(OnThrowButtonClicked);
            SetupDeckDisplay();
            UpdateUI();
        }

        void SetupDeckDisplay()
        {
            foreach (var kvp in runtimeDeck.currentDeck)
            {
                GameObject displayObj = Instantiate(displayObjPrefab, deckDisplayParent);
                displayObj.name = $"{kvp.Key.cardName}_Display";
                deckDisplayTexts[kvp.Key] = displayObj.GetComponent<TextMeshProUGUI>(); ;
            }
        }

        void UpdateUI()
        {
            // Update deck count
            int totalCards = runtimeDeck.GetTotalCardCount();
            deckCountText.text = totalCards.ToString();

            // Update individual card counts
            foreach (var kvp in deckDisplayTexts)
            {
                if (runtimeDeck.currentDeck.ContainsKey(kvp.Key))
                {
                    kvp.Value.text = $"{kvp.Key.cardName}: {runtimeDeck.currentDeck[kvp.Key]}";
                }
                else
                {
                    kvp.Value.text = $"{kvp.Key.cardName}: 0";
                }
            }
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

            // Throw cards with slight delays
            for (int i = 0; i < cardsToThrow.Count; i++)
            {
                ThrowCard(cardsToThrow[i]);
                yield return new WaitForSeconds(0.1f); // Small delay between throws
            }

            // Remove thrown cards from deck
            foreach (var card in cardsToThrow)
            {
                runtimeDeck.RemoveCard(card);
            }

            UpdateUI();

            GameStateManager.CanPlayerDrawLasso = true;
            while(thrownCards.Count > 0)
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
        }

        List<BaseCard> SelectCardsToThrow()
        {
            if (runtimeDeck.GetTotalCardCount() <= cardsToThrowPerTurn)
            {
                Logger.Log ("reshuffle!", shouldLog);

                List<BaseCard> discardCards = discardPile.GetAllCardsAsList();
                foreach (var card in discardCards)
                {
                    runtimeDeck.AddCard(card);
                }
                discardPile.RemoveAllCards();
            }

            List<BaseCard> allCards = runtimeDeck.GetAllCardsAsList();
            List<BaseCard> selectedCards = new List<BaseCard>();

            for (int i = 0; i < cardsToThrowPerTurn; i++)
            {
                int randomIndex = Random.Range(0, allCards.Count);
                selectedCards.Add(allCards[randomIndex]);
                discardPile.AddCard(allCards[randomIndex]);
                allCards.RemoveAt(randomIndex);
            }

            return selectedCards;
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

        void SetPlayerTurn(bool playerTurn)
        {
            TurnManager.ChangeTurn(playerTurn);
            throwButton.gameObject.SetActive(playerTurn);
        }
    }
}