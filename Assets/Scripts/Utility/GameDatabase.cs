using Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Database", menuName = "Scriptable Objects/GameDatabase")]
    public class GameDatabase : ScriptableObject
    {
        // TODO: limit the write access to this class only to GameDatabaseManager
        public List<Relic> relics;
        public List<Material> materials;
        public List<BaseCard> cards;
    }
}
