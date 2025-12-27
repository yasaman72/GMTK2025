using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public class MaterialIcon : MonoBehaviour
    {
        [Space]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _name;
        [ReadOnly, SerializeField] private Material _material;
        [ReadOnly, SerializeField] private TooltipTrigger _toolTipTrigger;

        public void Setup(Material material)
        {
            _material = material;

            if (_material != null)
            {
                if (_image != null)
                {
                    _image.sprite = _material.icon;
                    _image.color = _material.color;
                }
                if (_name != null)
                {
                    _name.text = _material.translatedName;
                }
            }

            if (_toolTipTrigger == null)
            {
                _toolTipTrigger = GetComponent<TooltipTrigger>();
            }

            _toolTipTrigger.SetLocalizedString(_material.materialName);
        }

        private void OnValidate()
        {
            if (_toolTipTrigger == null)
            {
                _toolTipTrigger = GetComponent<TooltipTrigger>();
            }
        }
    }
}
