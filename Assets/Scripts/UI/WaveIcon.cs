using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public class WaveIcon : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _starColor, _passeedColor;

        private void OnEnable()
        {
            _image.color = _starColor;
        }

        public void OnPass()
        {
            _image.color = _passeedColor;
        }
    }
}
