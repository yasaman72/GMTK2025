using Cards.ScriptableObjects;
using UnityEngine;

namespace Cards
{
    public class CardPrefab : MonoBehaviour
    {
        [Header("Card Reference")]
        public BaseCard cardData; // Reference to the scriptable object

        [Header("Visual")]
        public SpriteRenderer cardRenderer;
        public float destroyAfterTime = 10f; // Auto-destroy if not lassoed
        public Animation onActivateAnimation;

        [Header("Physics")]
        public bool isLassoed = false;

        private Rigidbody2D rb;


        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            ResetItem();
        }

        void Start()
        {
            // Set visual if card data is assigned
            if (cardData != null && cardRenderer != null && cardData.cardIcon != null)
            {
                cardRenderer.sprite = cardData.cardIcon;
            }

            // Auto-destroy after time
            Destroy(gameObject, destroyAfterTime);

            // Add some tag for lasso detection
            gameObject.tag = "ThrowableCard";
        }

        private void ResetItem()
        {
            isLassoed = false;

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1;

            if (cardRenderer != null)
            {
                cardRenderer.color = Color.white;
            }
        }

        public void OnLassoed()
        {
            isLassoed = true;
            Debug.Log($"Card {cardData?.cardName} was lassoed!");

            // Visual feedback
            if (cardRenderer != null)
            {
                cardRenderer.color = cardData.onSelectColor; // Show it's selected
            }

            // Stop physics
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }

            // Use the card
            if (cardData != null)
            {
                cardData.OnCardActivated(this, CardActivationCallback, this);
            }
        }

        private void CardActivationCallback()
        {
            Destroy(gameObject);
        }
    }
}