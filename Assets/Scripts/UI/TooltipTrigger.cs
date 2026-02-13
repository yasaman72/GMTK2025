using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace Deviloop
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private LocalizedString _tooltipText;
        public string TranslatedValue { get; private set; }
        private bool _isPointerOver;
        private Coroutine _hideCoroutine;


        private void OnDisable()
        {
            ResetTooltip();
        }

        private void ResetTooltip()
        {
            _isPointerOver = false;
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }

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

            // Cancel any pending hide coroutine so tooltip remains visible immediately on re-enter
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }

            TooltipManager.Instance.ShowTooltipUnderMouse(_tooltipText.GetLocalizedString());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;

            // If this component or GameObject is being disabled/destroyed, hide immediately.
            if (!isActiveAndEnabled || !gameObject.activeInHierarchy)
            {
                TooltipManager.Instance.HideTooltip();
                return;
            }

            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }


        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSecondsRealtime(0.1f);

            if (_isPointerOver)
                yield break;

            TooltipManager.Instance.HideTooltip();
            _hideCoroutine = null;
        }
    }
}
