using Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "AreasData", menuName = "Scriptable Objects/AreaData")]
    public class AreaData : ScriptableObject
    {
        public List<Area> Areas;

        public List<Area> Copy()
        {
            var _newAreasList = new List<Area>();
            foreach (var area in this.Areas)
            {
                var newArea = new Area
                {
                    AreaName = area.AreaName,
                    Encounters = new List<BaseEncounter>(area.Encounters),
                    BossEncounter = area.BossEncounter
                };
                _newAreasList.Add(newArea);
            }
            return _newAreasList;
        }
    }
}