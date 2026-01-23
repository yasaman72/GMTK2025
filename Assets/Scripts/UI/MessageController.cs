using System;
using UnityEngine;

namespace Deviloop
{
    public class MessageController : MonoBehaviour
    {
        public delegate void DisplayMessageDelegate(string message, float duration);
        public static DisplayMessageDelegate OnDisplayMessage;
        [SerializeField] private MesssageDisplayer _displayer;

        private void Start()
        {
            _displayer.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            OnDisplayMessage += DisplayMessage;
        }

        private void OnDisable()
        {
            OnDisplayMessage -= DisplayMessage;
        }

        public void DisplayMessage(string message, float duration)
        {
            _displayer.gameObject.SetActive(true);
            _displayer.ShowText(message);
            Invoke(nameof(HideMessage), duration);
        }

        private void HideMessage()
        {
            _displayer.gameObject.SetActive(false);
        }
    }
}
