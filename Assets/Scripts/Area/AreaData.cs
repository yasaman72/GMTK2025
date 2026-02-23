using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    [CreateAssetMenu(fileName = "AreasData", menuName = "Scriptable Objects/AreaData")]
    public class AreaData : ScriptableObject
    {
        public List<Area> Areas;


        public void Setup()
        {
            foreach (var area in Areas)
            {
                foreach (var e in area.Encounters)
                {
                    e.Reset();
                }
            }
        }

        public List<Area> Copy()
        {
            var _newAreasList = new List<Area>();
            foreach (var area in this.Areas)
            {
                var newArea = new Area
                {
                    AreaName = area.AreaName,
                    Encounters = new List<EncounterConfig>(area.Encounters),
                    BossEncounter = area.BossEncounter,
                    PassageEncounter = area.PassageEncounter
                };
                _newAreasList.Add(newArea);
            }
            return _newAreasList;
        }
    }
}