using UnityEngine;
using System.Collections.Generic;
using System;

namespace Deviloop
{
    public class EncounterManager : MonoBehaviour
    {
        // TODO: This should be in AreaManager
        public AreaData AllAreas;

        public static Area CurrentArea;
        private static int _currentAreaIndex = -1;

        private int _currentEncounterIndex = 0;
        private BaseEncounter _currentEncounter;

        public static Action OnEncounterFinished;

        private void OnEnable()
        {
            OnEncounterFinished += StartNextEncounter;
        }
        private void OnDisable()
        {
            OnEncounterFinished -= StartNextEncounter;
        }

        private void Start()
        {
            _currentAreaIndex = -1;
            StartNextArea();
        }

        public void StartNextArea()
        {
            _currentAreaIndex++;
            if (_currentAreaIndex >= AllAreas.Areas.Count)
            {
                Debug.Log("All areas completed. Game finished!");
                return;
            }
            
            CurrentArea = AllAreas.Areas[_currentAreaIndex];

            _currentEncounterIndex = 0;
            StartNextEncounter();
        }

        public void StartNextEncounter()
        {
            if ((_currentEncounterIndex == 0) || (_currentEncounterIndex == CurrentArea.MaxEncounters - 3))
            {
#if DEBUG
                if (_currentEncounterIndex == 0)
                    Debug.Log($"Starting encounters in area: {CurrentArea.AreaName}");
                else
                    Debug.Log("Starting a combat before pre boss shop encounter.");
#endif
                _currentEncounter = CurrentArea.GetRandomEncounterType<CombatEncounter>();
            }
            else if (_currentEncounterIndex == CurrentArea.MaxEncounters - 2)
            {
                Debug.Log("Starting pre boss shop encounter.");
                _currentEncounter = CurrentArea.GetRandomEncounterType<ShopEncounter>();
            }
            else if (_currentEncounterIndex == CurrentArea.MaxEncounters - 1)
            {
                Debug.Log("Starting boss encounter.");
                _currentEncounter = CurrentArea.BossEncounter;
            }
            else if (_currentEncounterIndex == CurrentArea.MaxEncounters)
            {
                Debug.Log("All encounters completed in area.");
                StartNextArea();
                return;
            }
            else
            {
                if (_currentEncounter is ShopEncounter)
                {
                    _currentEncounter = CurrentArea.GetRandomEncounterType<CombatEncounter>();
                }
                else
                {
                    _currentEncounter = CurrentArea.GetRandomEncounter();
                }

                Debug.Log($"Starting encounter {_currentEncounterIndex + 1}/{CurrentArea.MaxEncounters} in area {CurrentArea.AreaName}: {_currentEncounter.name}");

            }

            _currentEncounterIndex++;
            _currentEncounter.StartEncounter();
        }
    }
}
