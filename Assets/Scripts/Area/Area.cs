using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public class Area
    {
        public string AreaName;
        [Min(5)]
        public int MaxEncounters;
        public List<BaseEncounter> Encounters;
        public BaseEncounter BossEncounter;
        public BaseEncounter GetRandomEncounter()
        {
            int randomIndex = Random.Range(0, Encounters.Count);
            BaseEncounter randomEncounter = Encounters[randomIndex];

            return randomEncounter;
        }

        public BaseEncounter GetRandomEncounterType<T>()
        {
            BaseEncounter reservedEncounter = null;
            foreach (var encounter in Encounters)
            {
                if (encounter is T)
                {
                    reservedEncounter = encounter;
                    break;
                }
            }

            if(reservedEncounter == null)
            {
                Debug.LogError($"No encounter of type {typeof(T).Name} found in area {AreaName}.");
                return null;
            }

            BaseEncounter randomEncounter = GetRandomEncounter();

            // Try to get a random encounter of type T, but limit the number of attempts to avoid infinite loops
            for (int i = 0; !(randomEncounter is T) && i < 5; i++)
            {
                randomEncounter = GetRandomEncounter();
            }

            if(!(randomEncounter is T))
                return reservedEncounter;

            return randomEncounter;
        }
    }
}
