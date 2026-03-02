using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace Deviloop
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool _shouldShowDuringLasso = false;
        [SerializeField] private LocalizedString _tooltipText;

        private bool _isPointerOver;
        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            RenewToken();
        }

        private void OnDisable()
        {
            ResetTooltip();
        }

        private void OnDestroy()
        {
            CancelAndDispose();
        }

        private void ResetTooltip()
        {
            _isPointerOver = false;
            CancelAndDispose();

            if (TooltipManager.Instance != null)
                TooltipManager.Instance.HideTooltip();
        }

        public void SetLocalizedString(LocalizedString newString)
        {
            _tooltipText = newString;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;

            RenewToken();

            TooltipManager.Instance.ShowTooltipUnderMouse(
                _tooltipText.GetLocalizedString(), _shouldShowDuringLasso);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;

            if (!isActiveAndEnabled || !gameObject.activeInHierarchy)
            {
                TooltipManager.Instance.HideTooltip();
                return;
            }

            RenewToken();
            TooltipManager.Instance.HideTooltip();
        }

        private void RenewToken()
        {
            CancelAndDispose();
            _cts = new CancellationTokenSource();
        }

        private void CancelAndDispose()
        {
            if (_cts == null) return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}
