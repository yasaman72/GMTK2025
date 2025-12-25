using Cards.ScriptableObjects;
using UnityEngine;
using MaterialFlag = Cards.ScriptableObjects.MaterialType;

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
        public string materialName;
        public MaterialType type;
        public Color color;
        public Sprite icon;

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
