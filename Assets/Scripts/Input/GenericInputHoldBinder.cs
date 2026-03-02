using UnityEngine;
using UnityEngine.Events;

namespace Deviloop
{
    public class GenericInputHoldBinder : MonoBehaviour
    {
        [SerializeField] private KeyCode _keyCode;
        [SerializeField] private bool _isGameplay;
        [SerializeField] private UnityEvent _onHoldStartAction;
        [SerializeField] private UnityEvent _onReleaseStartAction;

        private void Update()
        {
            if (Input.GetKeyDown(_keyCode))
            {
                if (InputSettings.AreAllInputBlocked) return;
                if (InputSettings.IsGameplayInputBlocked && _isGameplay) return;

                _onHoldStartAction?.Invoke();
                InputSettings.AreAllInputBlocked = true;
            }
            else if (Input.GetKeyUp(_keyCode))
            {
                _onReleaseStartAction?.Invoke();
                InputSettings.AreAllInputBlocked = false;
            }
        }
    }
}
