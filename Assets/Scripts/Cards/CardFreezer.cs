using UnityEngine;

namespace Deviloop
{
    public class CardFreezer : MonoBehaviour
    {

        [SerializeField] private PlayerLassoManager _playerLassoManager;
        [SerializeField] private CardManager _cardManager;

        private static bool _isFrozen = false;
        public static bool IsCardFrozen => _isFrozen;

        public void ToggleCardFreeze()
        {
            if (CanFreeze() == false) return;

            _isFrozen = !_isFrozen;

            if (_isFrozen)
            {
                Freeze();
            }
            else
            {
                Unfreeze();
            }
        }

        private bool CanFreeze()
        {
            if (PlayerLassoManager.HasAlreadyDrawn) return false;
            return true;
        }

        private void Freeze()
        {
            Time.timeScale = 0f;

            GameStateManager.Instance.CanPlayerDrawLasso = false;
            _cardManager.FreezeCards();
        }

        private void Unfreeze()
        {
            Time.timeScale = 1f;

            GameStateManager.Instance.CanPlayerDrawLasso = true;
            _cardManager.UnfreezeCards();
        }
    }
}
