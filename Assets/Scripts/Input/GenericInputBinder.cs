using UnityEngine;
using UnityEngine.Events;

namespace Deviloop
{
    public class GenericInputBinder : MonoBehaviour
    {
        public static bool IsGameplayInputBlocked { get; set; }
        public static bool AreAllInputBlocked { get; set; }

        [SerializeField] private KeyCode _keyCode;
        [SerializeField] private bool _isGameplay;
        [SerializeField] private UnityEvent _action;

        private void Update()
        {
            if (AreAllInputBlocked) return;
            if (IsGameplayInputBlocked && _isGameplay) return;

            // TODO: replace with the new input manager and a event based system
            if (Input.GetKeyDown(_keyCode))
            {
                _action?.Invoke();
            }
        }
    }
}
