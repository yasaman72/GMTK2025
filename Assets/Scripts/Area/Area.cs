using System;
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
        [Serializable]
        public class EncounterProbability
        {
            public BaseEncounter Encounter;
            public int Probability;
            public bool IsStartingEncounter;
        }
        public List<EncounterProbability> Encounters;
        public BaseEncounter BossEncounter;

        public int TotalWeight { get; private set; } = -1;

        public BaseEncounter GetRandomEncounter(bool isStartingEncounter = false, List<BaseEncounter> encountersToSkip = null)
        {
            if (TotalWeight == -1)
            {
                TotalWeight = 0;
                foreach (var encounter in Encounters)
                {
                    TotalWeight += encounter.Probability;
                }
            }

            int randomIndex = 0;
            int randomValue = UnityEngine.Random.Range(0, TotalWeight);
            int cumulativeWeight = 0;

            for (int i = 0; i < Encounters.Count; i++)
            {
                cumulativeWeight += Encounters[i].Probability;
                if (randomValue < cumulativeWeight)
                {
                    if (isStartingEncounter && !Encounters[i].IsStartingEncounter)
                        continue; // Skip non-starting encounters if we need a starting encounter
                    
                    randomIndex = i;
                    break;
                }
            }

            BaseEncounter randomEncounter = Encounters[randomIndex].Encounter;

            if(encountersToSkip != null && encountersToSkip.Contains(randomEncounter))
            {
                return GetRandomEncounter(isStartingEncounter, encountersToSkip);
            }

            return randomEncounter;
        }

        public BaseEncounter GetRandomEncounterType<T>(bool isStartingEncounter, List<BaseEncounter> encountersToSkip = null)
        {
            BaseEncounter reservedEncounter = null;
            foreach (var encounter in Encounters)
            {
                if (encounter.Encounter is T)
                {
                    reservedEncounter = encounter.Encounter;
                    break;
                }
            }

            if (reservedEncounter == null)
            {
                Debug.LogError($"No encounter of type {typeof(T).Name} found in area {AreaName}.");
                return null;
            }

            BaseEncounter randomEncounter = null;

            // Try to get a random encounter of type T, but limit the number of attempts to avoid infinite loops
            for (int i = 0; !(randomEncounter is T) && i < 5; i++)
            {
                randomEncounter = GetRandomEncounter(isStartingEncounter, encountersToSkip);

                if (isStartingEncounter && !Encounters[i].IsStartingEncounter)
                    continue; // Skip non-starting encounters if we need a starting encounter

                if (encountersToSkip != null && encountersToSkip.Contains(randomEncounter))
                {
                    randomEncounter = null; // Ignore this encounter and try again
                }
            }

            if (!(randomEncounter is T))
                return reservedEncounter;

            return randomEncounter;
        }
    }
}
