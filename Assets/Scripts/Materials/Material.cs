using UnityEngine;
using UnityEngine.Localization;
using MaterialFlag = Cards.MaterialType;

namespace Deviloop
{

    public enum MaterialType
    {
        Unknown = MaterialFlag.Unknown,
        Metal = MaterialFlag.Metal,
        Stone = MaterialFlag.Stone,
        Wood = MaterialFlag.Wood,
        Fabric = MaterialFlag.Fabric,
        Flesh = MaterialFlag.Flesh,
        Glass = MaterialFlag.Glass,
        Explosive = MaterialFlag.Explosive,
    }

    [CreateAssetMenu(fileName = "Material", menuName = "Scriptable Objects/Material")]
    public class Material : ScriptableObject
    {
        public LocalizedString materialName;
        [HideInInspector] public string translatedName;
        public MaterialType type;
        public Color color;
        public Sprite icon; 
        
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

        private MaterialFlag ToFlag(MaterialType type)
        {
            return System.Enum.TryParse(type.ToString(), out MaterialFlag flag)
                ? flag
                : MaterialFlag.Unknown;
        }

        public bool CompareMaterials(MaterialFlag flag)
        {
            var enumToflag = ToFlag(type);
            return (flag & enumToflag) == enumToflag;
        }
    }
}
