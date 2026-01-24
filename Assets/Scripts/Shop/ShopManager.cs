using System;
using TMPro;
using UnityEngine;

namespace Deviloop
{
    public class ShopManager : MonoBehaviour
    {
        public static Action<ShopData> OnShopStartEvent;
        public static Action OnShopFinishedEvent;

        [SerializeField] private TextMeshProUGUI _shopkeeperDialogue;
        [SerializeField] private GameObject _dialogueBubble;

        private void Awake()
        {
            OnShopStartEvent += StartShop;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnShopStartEvent -= StartShop;
        }

        private void OnEnable()
        {
            PlayerInventory.OnNotEnoughGold += OnNotEnoughGold;
        }

        private void OnDisable()
        {
            PlayerInventory.OnNotEnoughGold -= OnNotEnoughGold;
        }

        private void OnNotEnoughGold()
        {
            _dialogueBubble.SetActive(true);
            _shopkeeperDialogue.text = "You don't have enough gold!";
            CancelInvoke(nameof(HideDialogue));
            Invoke(nameof(HideDialogue), 2f);
        }

        private void HideDialogue()
        {
            _dialogueBubble.SetActive(false);
        }

        private void StartShop(ShopData shopData)
        {
            gameObject.SetActive(true);
            ShopUI.OpenShopAction?.Invoke(shopData);
            ShopUI.OnShopClosedEvent += FinishShop;
        }

        private void FinishShop()
        {
            OnShopFinishedEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
