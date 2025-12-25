using Cards;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Database", menuName = "Scriptable Objects/GameDatabase")]
    public class GameDatabase : ScriptableObject
    {
        public Material[] materials;
        public BaseCard[] Cards;
    }
}
