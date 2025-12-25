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
        [SerializeField] private Material _testMaterial;

        private Material _material;

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
                    _name.text = _material.materialName;
                }
            }

        }

        private void OnValidate()
        {
            if (_testMaterial != null)
            {
                Setup(_testMaterial);
            }
        }
    }
}
