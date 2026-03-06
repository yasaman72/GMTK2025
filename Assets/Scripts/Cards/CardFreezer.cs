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

        public void Freeze()
        {
            if (CanFreeze() == false) return;

            Time.timeScale = 0f;

            GameStateManager.Instance.CanPlayerDrawLasso = false;
            _cardManager.FreezeCards();
        }

        public void Unfreeze()
        {
            Time.timeScale = 1f;

            GameStateManager.Instance.CanPlayerDrawLasso = true;
            _cardManager.UnfreezeCards();
        }
    }
}
