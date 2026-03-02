using UnityEngine;
using UnityEngine.Events;

namespace Deviloop
{
    public class GenericInputBinder : MonoBehaviour
    {
        [SerializeField] private KeyCode _keyCode;
        [SerializeField] private bool _isGameplay;
        [SerializeField] private UnityEvent _action;

        private void Update()
        {
            if (InputSettings.AreAllInputBlocked) return;
            if (InputSettings.IsGameplayInputBlocked && _isGameplay) return;

            // TODO: replace with the new input manager and a event based system
            if (Input.GetKeyDown(_keyCode))
            {
                _action?.Invoke();
            }
        }
    }
}
