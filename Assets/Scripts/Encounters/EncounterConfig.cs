using System;

namespace Deviloop
{
    [Serializable]
    public class EncounterConfig
    {
        public BaseEncounter Encounter;
        public int Probability;
        public bool IsStartingEncounter;

        public bool ShouldSpawnRegularly;
        [ShowIf(nameof(ShouldSpawnRegularly))]
        public int SpawnAfterEveryXEncounter;
        [ReadOnly]
        [ShowIf(nameof(ShouldSpawnRegularly))]
        public int EncountersSinceLastSpawn = 0;
        private int _lastCheckedEncounterIndex = -1;

        public void Reset()
        {
            EncountersSinceLastSpawn = 0;
        }

        public bool ShouldSpawnEncountersNow()
        {
            if (!ShouldSpawnRegularly) return false;

            // Only check for spawning once per encounter
            if (_lastCheckedEncounterIndex == EncounterManager.CurrentEncounterIndex) return false;
            _lastCheckedEncounterIndex = EncounterManager.CurrentEncounterIndex;

            EncountersSinceLastSpawn++;
            if (EncountersSinceLastSpawn > SpawnAfterEveryXEncounter)
            {
                EncountersSinceLastSpawn = 0;
                return true;
            }
            return false;
        }
    }
}
