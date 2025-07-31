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
    
        [Header("Physics")]
        public bool isLassoed = false;
    
        private Rigidbody rb;
        private Collider col;
    
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
        
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
    
        public void OnLassoed()
        {
            if (isLassoed) return;
        
            isLassoed = true;
            Debug.Log($"Card {cardData?.cardName} was lassoed!");
        
            // Visual feedback
            if (cardRenderer != null)
            {
                cardRenderer.color = Color.green; // Show it's selected
            }
        
            // Stop physics
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        
            // Use the card
            if (cardData != null)
            {
                cardData.OnCardActivated();
            }
        }
    
        void OnMouseDown()
        {
            // Placeholder for lasso selection - can be replaced with proper lasso mechanic
            OnLassoed();
        }
    
        // For lasso system to check if this card is inside the lasso
        public bool IsInsideLasso(Vector2[] lassoPoints)
        {
            // TODO: Implement proper point-in-polygon detection
            // For now, return false (placeholder)
            return false;
        }
    }
}