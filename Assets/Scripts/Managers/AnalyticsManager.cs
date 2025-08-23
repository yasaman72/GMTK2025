using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;

namespace Deviloop
{
    public class AnalyticsManager : CustomMonoBehavior
    {
        public static Action<string, IDictionary<string, object>> SendCustomEventAction;

        [SerializeField] private bool _isAnalyticsEnabled = true;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            GameAnalytics.Initialize();
        }
        private void OnEnable()
        {
            SendCustomEventAction += SendCustomEvent;
        }

        private void OnDisable()
        {
            SendCustomEventAction -= SendCustomEvent;
        }

        public void SendCustomEvent(string eventName, IDictionary<string, object> eventData)
        {
            if (!_isAnalyticsEnabled)
                return;

            try
            {
                GameAnalytics.NewDesignEvent(eventName, eventData);
                Logger.Log($"Analytics event {eventName} sent successfully.", shouldLog);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to send analytics event {eventName}. Exception: {e.Message}");
            }
        }
    }
}