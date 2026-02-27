using UnityEngine;
using UnityEngine.Localization;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Material", menuName = "Scriptable Objects/Material")]
    public class MaterialData : ScriptableObject
    {
        public LocalizedString materialName;
        [HideInInspector] public string translatedName;
        public MaterialType type;
        public Color color;
        public Sprite icon; 
        public Rarity rarity; 
        
        [DeveloperNotes, SerializeField]
        private string developerNotes;


        private void OnValidate()
        {
            translatedName = materialName.GetLocalizedString();
            materialName.StringChanged += ValueChanged;
        }

        private void ValueChanged(string value)
        {
            translatedName = value;
        }

        private F_MaterialType ToFlag(MaterialType type)
        {
            return System.Enum.TryParse(type.ToString(), out F_MaterialType flag)
                ? flag
                : F_MaterialType.Unknown;
        }

        public bool CompareMaterials(F_MaterialType flag)
        {
            var enumToflag = ToFlag(type);
            return (flag & enumToflag) == enumToflag;
        }
    }
}
