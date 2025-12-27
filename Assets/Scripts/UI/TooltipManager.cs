using TMPro;
using UnityEngine;

namespace Deviloop
{
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager Instance;
        [SerializeField] private GameObject _tooltipPrefab;

        private GameObject _tooltip;
        private TextMeshProUGUI _text;
        private Canvas _canvas;
        private RectTransform _tooltipRect;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
            gameObject.SetActive(true);
            _canvas = GetComponentInParent<Canvas>();
            _tooltip = Instantiate(_tooltipPrefab, _canvas.transform);
            _text = _tooltip.GetComponentInChildren<TextMeshProUGUI>();
            _tooltipRect = _tooltip.GetComponent<RectTransform>();
            _tooltip.SetActive(false);
        }

        public void ShowTooltipInPosition(string content, Vector2 position)
        {
            ShowTooltip(content, position);
        }

        public void ShowTooltipUnderMouse(string content)
        {
            ShowTooltip(content, Input.mousePosition);
        }

        private void ShowTooltip(string content, Vector2 position)
        {
            if(content == null || content == "")
            {
                Debug.LogWarning("Tooltip content is null or empty, hiding tooltip.");
                HideTooltip();
                return;
            }

            _text.SetText(content);
            _text.ForceMeshUpdate();
            _tooltip.SetActive(true);

            // TODO: clamp the tooltip position to be within the canvas bounds

            // if tooltip is on the right side of the screen, move the pivot to the right
            var canvasSize = _canvas.renderingDisplaySize;
            if(position.x > canvasSize.x / 2)
            {
                _tooltipRect.pivot = new Vector2(1, .5f);
            }
            else
            {
                _tooltipRect.pivot = new Vector2(0, .5f);
            }

            _tooltip.transform.position = position;

        }

        public void HideTooltip()
        {
            _tooltip.SetActive(false);
        }
    }
}
