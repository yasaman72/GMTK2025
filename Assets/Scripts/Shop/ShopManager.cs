using System;
using UnityEngine;

namespace Deviloop
{
    public class ShopManager : MonoBehaviour
    {
        public static Action<ShopData> OnShopStartEvent;
        public static Action OnShopFinishedEvent;

        private void Awake()
        {
            OnShopStartEvent += StartShop;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnShopStartEvent -= StartShop;
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
