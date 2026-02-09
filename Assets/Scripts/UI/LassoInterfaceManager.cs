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
            PlayerLassoManager.OnLoopClosed += OnLoopClosed;
            GameStateManager.OnPlayerClickedThrowButton += OnPlayerDrawTurnStart;
        }

        private void OnDisable()
        {
            PlayerLassoManager.OnLassoSizeChanged -= UpdateSlider;
            PlayerLassoManager.OnLoopClosed -= OnLoopClosed;
            GameStateManager.OnPlayerClickedThrowButton -= OnPlayerDrawTurnStart;
        }

        private void OnLoopClosed()
        {
            _holder.SetActive(false);
        }

        private void OnPlayerDrawTurnStart()
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
