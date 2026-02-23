using System;
using TMPro;
using UnityEngine;

namespace Deviloop
{
    public class ShopManager : MonoBehaviour, IInitiatable
    {
        public static Action<ShopData> OnShopStartEvent;
        public static Action OnShopFinishedEvent;

        [SerializeField] private TextMeshProUGUI _shopkeeperDialogue;
        [SerializeField] private GameObject _dialogueBubble;


        public void Initiate()
        {
            OnShopStartEvent += StartShop;
            gameObject.SetActive(false);
        }

        public void Deactivate()
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
            ShopUI.OnShopClosedEvent -= FinishShop;

            OnShopFinishedEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
