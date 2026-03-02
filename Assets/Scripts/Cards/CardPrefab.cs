using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Deviloop
{
    public class CardPrefab : MonoBehaviour, IPoolable, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Components")]
        [SerializeField] private SpriteRenderer _cardRenderer;
        [SerializeField] private TooltipTrigger _tooltipTrigger;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PolygonCollider2D _collider;
        [SerializeField] private Color _freezeColor;
        [SerializeField] private Color _onFreezeOutlineColor;

        [ReadOnly]
        public bool isLassoed = false;
        [ReadOnly, SerializeField]
        private BaseCard cardData;
        public BaseCard CardData { get { return cardData; } }

        public void OnSpawned()
        {
            isLassoed = false;

            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.gravityScale = 1;

            if (_cardRenderer != null)
            {
                _cardRenderer.color = Color.white;
                _cardRenderer.material.SetColor("_OutlineColor", Color.white);
                _cardRenderer.material.SetFloat("_OutlineWidth", 2);
            }
        }

        public void OnDespawned()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void InitializeCard(BaseCard card)
        {
            cardData = card;
            _cardRenderer.sprite = cardData.cardIcon;
            _collider.CreateFromSprite(cardData.cardIcon);
            cardData.AddComponent(gameObject);
            transform.localScale = cardData.spriteScale;
            gameObject.name = cardData.name;

            _tooltipTrigger.SetLocalizedString(card.description);
            _tooltipTrigger.enabled = false;
        }

        public void OnLassoed()
        {
            isLassoed = true;

            if (cardData != null)
            {
                cardData.ApplyOnLassoeEffects(this);
            }

            // Visual feedback
            if (_cardRenderer != null)
            {
                _cardRenderer.color = cardData.OnSelectColor; // Show it's selected
            }

            // Stop physics
            if (_rb != null)
            {
                _rb.bodyType = RigidbodyType2D.Static;
            }
        }

        public void OnActivate()
        {
            if (cardData != null)
            {
                cardData.UseCard(CardActivationCallback, this);
                _cardRenderer.material.SetColor("_OutlineColor", Color.red);
                _cardRenderer.material.SetFloat("_OutlineWidth", 10);
            }
        }

        public void OnDropedForBeingExtra()
        {
            _cardRenderer.color = new Color(.5f, .5f, .5f, .5f);
            transform.DOShakePosition(1f, 0.2f, 10, 90, false, true);
            transform.DOScale(Vector3.zero, 0.5f).SetDelay(1f).OnComplete(
                () => PoolManager.Instance.GetPool<CardPrefab>(this).ReturnToPool(this));
        }

        private void CardActivationCallback()
        {
            PoolManager.Instance.GetPool<CardPrefab>(this).ReturnToPool(this);
        }

        public void OnCardDroppedOut()
        {
            //if (cardData is ComboCard)
            //{
            //    PlayerComboManager.OnPlayerComboBreak?.Invoke();
            //}

            cardData.ApplyOnDropEffects(this);

            PoolManager.Instance.GetPool<CardPrefab>(this).ReturnToPool(this);
        }
    
        public void FreezeCard()
        {
            _cardRenderer.color = _freezeColor;
            _tooltipTrigger.enabled = true;

        }

        public void UnfreezeCard()
        {
            _tooltipTrigger.enabled = false;

            if (_cardRenderer != null)
            {
                _cardRenderer.color = Color.white;
                _cardRenderer.material.SetColor("_OutlineColor", Color.white);
                _cardRenderer.material.SetFloat("_OutlineWidth", 2);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CardFreezer.IsCardFrozen) return;

            _cardRenderer.material.SetColor("_OutlineColor", _onFreezeOutlineColor);
            _cardRenderer.material.SetFloat("_OutlineWidth", 10);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!CardFreezer.IsCardFrozen) return;

            _cardRenderer.material.SetColor("_OutlineColor", Color.white);
            _cardRenderer.material.SetFloat("_OutlineWidth", 5);
        }
    }
}