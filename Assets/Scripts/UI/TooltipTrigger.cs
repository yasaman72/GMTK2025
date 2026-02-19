using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace Deviloop
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
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
                _tooltipText.GetLocalizedString());
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
            _ = HideAfterDelayAsync(_cts.Token);
        }

        private async Task HideAfterDelayAsync(CancellationToken token)
        {
            try
            {
                await Awaitable.WaitForSecondsAsync(0.1f, token);

                if (token.IsCancellationRequested || _isPointerOver)
                    return;

                TooltipManager.Instance.HideTooltip();
            }
            catch (OperationCanceledException) { }
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
