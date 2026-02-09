using UnityEngine;

namespace Deviloop
{
    public class FollowMousePosition : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        private Camera _mainCamera;
        private RectTransform _rectTransform;
        private bool _isUI;
        private Camera _uiCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
            _isUI = _rectTransform != null && _canvas != null;

            if (_isUI)
            {
                _uiCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null
                    : _canvas.worldCamera;
            }
        }

        private void Update()
        {
            if (_isUI)
            {
                FollowUI();
            }
            else
            {
                FollowWorld();
            }
        }

        private void FollowUI()
        {
            RectTransform canvasRect = (RectTransform)_canvas.transform;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                Input.mousePosition,
                _uiCamera,
                out Vector2 localPoint))
            {
                _rectTransform.anchoredPosition = localPoint;
            }
        }

        private void FollowWorld()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(_mainCamera.transform.position.z);

            transform.position = _mainCamera.ScreenToWorldPoint(mousePosition);
        }
    }
}
