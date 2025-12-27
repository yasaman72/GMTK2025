using Unity.VisualScripting.YamlDotNet.Core.Tokens;
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

        void Start()
        {
            _tooltipText.StringChanged += ValueChanged;
        }

        private void ValueChanged(string value)
        {
            TranslatedValue = value;
        }

        public void SetLocalizedString(LocalizedString newString)
        {
            _tooltipText = newString;
            TranslatedValue = _tooltipText.GetLocalizedString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
            TooltipManager.Instance.ShowTooltipUnderMouse(TranslatedValue);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            // Delay hiding the tooltip to avoid flickering
            Invoke(nameof(HideAfterDelay), .5f);
        }

        public void HideAfterDelay()
        {
            if (_isPointerOver)
                return;
            TooltipManager.Instance.HideTooltip();
        }
    }
}
