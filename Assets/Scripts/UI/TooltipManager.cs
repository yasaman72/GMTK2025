using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        [SerializeField] private GameObject _tooltipPrefab;
        [SerializeField] private Vector2 _tooltipOffset;

        private GameObject _tooltip;
        private RectTransform _tooltipRec;
        private Canvas _canvas;
        private RectTransform _tooltipRect;
        private MesssageDisplayer _messsageDisplayer;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(true);
            _canvas = GetComponentInParent<Canvas>();
            _tooltip = Instantiate(_tooltipPrefab, _canvas.transform);
            _messsageDisplayer = _tooltip.GetComponentInChildren<MesssageDisplayer>();
            _tooltipRect = _tooltip.GetComponent<RectTransform>();
            _tooltip.SetActive(false);

            _tooltipRec = _tooltip.GetComponent<RectTransform>();
        }

        public void ShowTooltipInPosition(string content, Vector2 position)
        {
            StartCoroutine(ShowTooltip(content, position));
        }

        public void ShowTooltipUnderMouse(string content)
        {
            StartCoroutine(ShowTooltip(content, Input.mousePosition));
        }

        private IEnumerator ShowTooltip(string content, Vector2 position)
        {
            if (!PlayerLassoManager.HasAlreadyDrawn) yield break;

            _tooltip.SetActive(true);

            if (content == null || content == "")
            {
                Debug.LogWarning("Tooltip content is null or empty, hiding tooltip.");
                HideTooltip();
                yield break;
            }


            // TODO: clamp the tooltip position to be within the canvas bounds

            // if tooltip is on the right side of the screen, move the pivot to the right
            var canvasSize = _canvas.renderingDisplaySize;
            if (position.x > canvasSize.x / 2)
            {
                _tooltipRect.pivot = new Vector2(1, .5f);
            }
            else
            {
                _tooltipRect.pivot = new Vector2(0, .5f);
            }

            _tooltip.transform.position = position + _tooltipOffset;

            _messsageDisplayer.ShowText(content);
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_tooltipRec);
        }

        public void HideTooltip()
        {
            if (_tooltip == null)
                return;
            _tooltip.SetActive(false);
        }

        private void Update()
        {
            if (_tooltip.activeSelf)
            {
                Vector2 position = Input.mousePosition;
                var canvasSize = _canvas.renderingDisplaySize;
                if (position.x > canvasSize.x / 2)
                {
                    // on right side
                    _tooltipRect.pivot = new Vector2(1, .5f);
                    _tooltip.transform.position = new Vector2(position.x - _tooltipOffset.x, position.y + _tooltipOffset.y);
                }
                else
                {
                    // on left side
                    _tooltipRect.pivot = new Vector2(0, .5f);
                    _tooltip.transform.position = new Vector2(position.x + _tooltipOffset.x, position.y + _tooltipOffset.y);
                }
            }
        }
    }
}
