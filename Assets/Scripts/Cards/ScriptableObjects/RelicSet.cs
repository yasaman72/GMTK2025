using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "RelicSet", menuName = "Scriptable Objects/RelicSet")]
    public class RelicSet : ScriptableObject
    {
        [SerializeField] private Relic[] _relics;
        public Relic[] Relics => _relics;
    }
}
