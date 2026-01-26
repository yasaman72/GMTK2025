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
            [Range(1, 100)]
            public int Probability;
        }
        [Header("Sum of all Probabilities should be 100.")]
        public List<EncounterProbability> Encounters;
        public BaseEncounter BossEncounter;

        public int TotalWeight { get; private set; } = -1;

        public BaseEncounter GetRandomEncounter(BaseEncounter[] encountersToIgnore = null)
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
                    randomIndex = i;
                    break;
                }
            }

            BaseEncounter randomEncounter = Encounters[randomIndex].Encounter;

            if(encountersToIgnore != null && Array.Exists(encountersToIgnore, e => e == randomEncounter))
            {
                return GetRandomEncounter(encountersToIgnore);
            }

            return randomEncounter;
        }

        public BaseEncounter GetRandomEncounterType<T>(BaseEncounter[] encountersToIgnore = null)
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
                randomEncounter = GetRandomEncounter();
                if (encountersToIgnore != null && Array.Exists(encountersToIgnore, e => e == randomEncounter))
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
