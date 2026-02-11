using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "Stats_Enemy_", menuName = "Scriptable Objects/Combat/EnemyData", order = 1)]
    public class EnemyData : CombatCharacterStats
    {
        public GameObject prefab;
        public List<EnemyAction> EnemyActions;

        [DeveloperNotes, SerializeField]
        private string _developerNotes;
    }
}
