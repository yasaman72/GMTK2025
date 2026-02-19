using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public class LassoInterfaceManager : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject _holder;

        private void OnEnable()
        {
            _holder.SetActive(false);
            PlayerLassoManager.OnLassoSizeChanged += UpdateSlider;
            PlayerLassoManager.OnLoopClosed += DisableUI;
            CardManager.OnPlayerClickedThrowButton += EnableUI;
            TurnManager.OnTurnChanged += OnTurnChanged;
            CombatManager.OnAfterAllEnemiesDefeated += DisableUI;
        }

        private void OnDisable()
        {
            PlayerLassoManager.OnLassoSizeChanged -= UpdateSlider;
            PlayerLassoManager.OnLoopClosed -= DisableUI;
            CardManager.OnPlayerClickedThrowButton -= EnableUI;
            TurnManager.OnTurnChanged -= OnTurnChanged;
            CombatManager.OnAfterAllEnemiesDefeated -= DisableUI;
        }

        private void OnTurnChanged(TurnManager.ETurnMode mode)
        {
            if (mode != TurnManager.ETurnMode.Player)
                _holder.SetActive(false);
        }

        private void DisableUI()
        {
            _holder.SetActive(false);
        }

        private void EnableUI()
        {
            _holder.SetActive(true);
            _slider.value = 1;
        }

        private void UpdateSlider(int maxSize, int currentSize)
        {
            _slider.value = (float)(maxSize - currentSize) / maxSize;
        }
    }
}
