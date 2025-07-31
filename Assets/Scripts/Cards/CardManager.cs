using System.Collections;
using System.Collections.Generic;
using Cards.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Cards
{
    public class CardManager : MonoBehaviour
    {
        [Header("Deck Configuration")]
        public CardDeck baseDeck; // Assign in inspector
        private CardDeck runtimeDeck; // Copy for gameplay
    
        [Header("Throwing Settings")]
        public int cardsToThrowPerTurn = 5;
        public Transform throwOrigin; // Bottom of screen
        public Vector2 throwForceRange = new Vector2(5f, 15f);
        public Vector2 throwAngleRange = new Vector2(60f, 120f); // Degrees
    
        [Header("UI Elements")]
        public Button throwButton;
        public TextMeshProUGUI deckCountText;
        public Transform deckDisplayParent; // For showing remaining cards
        [SerializeField] GameObject displayObjPrefab;
    
        [Header("Turn Management")]
        public bool isPlayerTurn = true;
        public float enemyTurnDuration = 2f;
    
        // Runtime variables
        private List<GameObject> thrownCards = new();
        private Dictionary<BaseCard, TextMeshProUGUI> deckDisplayTexts = new();
    
        void Start()
        {
            InitializeGame();
        }
    
        void InitializeGame()
        {
            // Create runtime copy of deck
            runtimeDeck = Instantiate(baseDeck);
            runtimeDeck.InitializeDeck();
        
            // Setup UI
            throwButton.onClick.AddListener(OnThrowButtonClicked);
            SetupDeckDisplay();
            UpdateUI();
        
            // Start with player turn
            SetPlayerTurn(true);
        }
    
        void SetupDeckDisplay()
        {
            foreach (var kvp in runtimeDeck.currentDeck)
            {
                GameObject displayObj = Instantiate(displayObjPrefab, deckDisplayParent);
                displayObj.name = $"{kvp.Key.cardName}_Display";
                deckDisplayTexts[kvp.Key] = displayObj.GetComponent<TextMeshProUGUI>();;
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
            if (!isPlayerTurn) return;
        
            StartCoroutine(ThrowCardsSequence());
        }
    
        IEnumerator ThrowCardsSequence()
        {
            // Disable button during throwing
            throwButton.interactable = false;
        
            // Get cards to throw
            List<BaseCard> cardsToThrow = SelectCardsToThrow();
        
            if (cardsToThrow.Count == 0)
            {
                Debug.Log("No cards left to throw!");
                throwButton.interactable = true;
                yield break;
            }
        
            // Clear previous thrown cards
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
        
            // Wait for player to lasso cards (placeholder)
            yield return StartCoroutine(WaitForPlayerLasso());
        
            // Switch to enemy turn
            SetPlayerTurn(false);
            yield return new WaitForSeconds(enemyTurnDuration);
        
            // Back to player turn
            SetPlayerTurn(true);
        }
    
        List<BaseCard> SelectCardsToThrow()
        {
            List<BaseCard> allCards = runtimeDeck.GetAllCardsAsList();
            List<BaseCard> selectedCards = new List<BaseCard>();
        
            // Randomly select cards to throw
            int cardsToSelect = Mathf.Min(cardsToThrowPerTurn, allCards.Count);
        
            for (int i = 0; i < cardsToSelect; i++)
            {
                if (allCards.Count > 0)
                {
                    int randomIndex = Random.Range(0, allCards.Count);
                    selectedCards.Add(allCards[randomIndex]);
                    allCards.RemoveAt(randomIndex);
                }
            }
        
            return selectedCards;
        }
    
        void ThrowCard(BaseCard card)
        {
            if (card.cardPrefab == null)
            {
                Debug.LogWarning($"No prefab assigned for {card.cardName}");
                return;
            }
        
            // Instantiate card prefab
            GameObject thrownCard = Instantiate(card.cardPrefab, throwOrigin.position, Quaternion.identity);
            thrownCards.Add(thrownCard);
        
            // Add rigidbody if not present
            Rigidbody rb = thrownCard.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = thrownCard.AddComponent<Rigidbody>();
            }
        
            // Calculate throw force
            float angle = Random.Range(throwAngleRange.x, throwAngleRange.y) * Mathf.Deg2Rad;
            float force = Random.Range(throwForceRange.x, throwForceRange.y);
        
            Vector3 throwDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            rb.AddForce(throwDirection * force, ForceMode.Impulse);
        
            // Add random rotation
            rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        
            Debug.Log($"Threw {card.cardName}");
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
    
        IEnumerator WaitForPlayerLasso()
        {
            Debug.Log("Waiting for player to lasso cards... (Placeholder - 3 seconds)");
        
            // Placeholder: Wait for 3 seconds
            // TODO: Replace with actual lasso mechanic
            yield return new WaitForSeconds(3f);
        
            // Placeholder: Simulate using some cards
            Debug.Log("Player finished lassoing cards (simulated)");
        
            // TODO: Process lassoed cards here
            // For now, just clear thrown cards
            ClearThrownCards();
        }
    
        void SetPlayerTurn(bool playerTurn)
        {
            isPlayerTurn = playerTurn;
            throwButton.interactable = playerTurn;
        
            if (playerTurn)
            {
                Debug.Log("Player's turn!");
            }
            else
            {
                Debug.Log("Enemy's turn!");
            }
        }
    
        // Public method for other systems to trigger enemy turn end
        public void EndEnemyTurn()
        {
            if (!isPlayerTurn)
            {
                SetPlayerTurn(true);
            }
        }
    }
}