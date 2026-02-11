using Deviloop.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace Deviloop
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

        private Rigidbody2D rb;


        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            ResetItem();
        }

        void Start()
        {
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
                cardRenderer.material.SetColor("_OutlineColor", Color.white);
                cardRenderer.material.SetFloat("_OutlineWidth", 2);
            }
        }

        public void OnLassoed()
        {
            isLassoed = true;

            // Visual feedback
            if (cardRenderer != null)
            {
                cardRenderer.color = cardData.OnSelectColor; // Show it's selected
            }

            // Stop physics
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }

        public void OnActivate()
        {
            if (cardData != null)
            {
                cardData.OnCardActivated(this, CardActivationCallback, this);
                cardRenderer.material.SetColor("_OutlineColor", Color.red);
                cardRenderer.material.SetFloat("_OutlineWidth", 10);
            }
        }

        public void OnDropedForBeingExtra()
        {
            cardRenderer.color = new Color(.5f, .5f, .5f, .5f);
            transform.DOShakePosition(1f, 0.2f, 10, 90, false, true);
            transform.DOScale(Vector3.zero, 0.5f).SetDelay(1f).OnComplete(() => Destroy(gameObject));
        }

        private void CardActivationCallback()
        {
            Destroy(gameObject);
        }

        public void OnCardDroppedOut()
        {
            if (cardData is ComboCard)
            {
                PlayerComboManager.OnPlayerComboBreak?.Invoke();
            }

            Destroy(gameObject);
        }
    }
}