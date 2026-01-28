using UnityEngine;
using System.Collections.Generic;
using System;

namespace Deviloop
{
    public class EncounterManager : MonoBehaviour
    {
        [Tooltip("Enable endless mode for testing purposes.")]
        [SerializeField] private bool _endlessMode = true;

        // TODO: This should be in AreaManager
        [SerializeField] private AreaData _allAreas;
        [SerializeField] private EncounterSelectionUI _encounterSelectionUI;

        public static Area CurrentArea;
        private static int _currentAreaIndex = -1;

        private int _currentEncounterIndex = 0;
        public static BaseEncounter CurrentEncounter;

        public static Action OnEncounterFinished;

        private void OnEnable()
        {
            OnEncounterFinished += EncounterFinished;
        }
        private void OnDisable()
        {
            OnEncounterFinished -= EncounterFinished;
        }

        private void Start()
        {
            _currentAreaIndex = -1;
            StartNextArea();
        }

        public void StartNextArea()
        {
            _currentAreaIndex++;


            if (!_endlessMode && _currentAreaIndex >= _allAreas.Areas.Count)
            {
                Debug.Log("All areas completed. Game finished!");
                return;
            }

            CurrentArea = _allAreas.Areas[_currentAreaIndex];

            _currentEncounterIndex = 0;
            EncounterFinished();
        }

        public void EncounterFinished()
        {
            if (_currentEncounterIndex == 0)
            {
                Debug.Log($"Starting encounters in area: {CurrentArea.AreaName}");

                CurrentEncounter = CurrentArea.GetRandomEncounterType<CombatEncounter>();
                _currentEncounterIndex++;
                CurrentEncounter.StartEncounter();
                return;
            }

            if (_currentEncounterIndex == CurrentArea.MaxEncounters)
            {
                // TODO: put this endless somewhere better
                if (_endlessMode)
                {
                    Logger.Log("Endless mode enabled - restarting area.");
                    _currentEncounterIndex = 0;
                    EncounterFinished();
                    return;
                }
                Debug.Log("All encounters completed in area.");
                StartNextArea();
                return;
            }

            ShowEncounterSelectionUI();
        }

        // TODO: Refactor this method to make it more modular. For example use a list in the AreaData to pick the next encounters.
        // Too many nested conditions
        public void ShowEncounterSelectionUI()
        {
            List<BaseEncounter> nextEncounters = new List<BaseEncounter>();

            if (!_endlessMode && _currentEncounterIndex == CurrentArea.MaxEncounters - 3)
            {
                Debug.Log("Starting a combat before pre boss shop encounter.");

                // TODO: check that thy're not the same encounter
                nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>());
                nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>());
            }
            else if (!_endlessMode && _currentEncounterIndex == CurrentArea.MaxEncounters - 2)
            {
                Debug.Log("Starting pre boss shop/rest encounter.");

                nextEncounters.Add(CurrentArea.GetRandomEncounterType<ShopEncounter>());
                //nextEncounters.Add(CurrentArea.GetRandomEncounterType<RestEncounter>());
            }
            else if (!_endlessMode && _currentEncounterIndex == CurrentArea.MaxEncounters - 1)
            {
                Debug.Log("Starting boss encounter.");
                nextEncounters.Add(CurrentArea.BossEncounter);
            }
            else
            {
                List<BaseEncounter> currentEncounters = new List<BaseEncounter>();

                // Don't allow two shops in a row
                if (CurrentEncounter is ShopEncounter)
                    currentEncounters.Add(CurrentEncounter);

                nextEncounters.Add(CurrentArea.GetRandomEncounter(currentEncounters));
                currentEncounters.Add(nextEncounters[0]);
                nextEncounters.Add(CurrentArea.GetRandomEncounter(currentEncounters));

                Debug.Log($"Starting encounter {_currentEncounterIndex + 1}/{CurrentArea.MaxEncounters} in area {CurrentArea.AreaName}: {CurrentEncounter.name}");
            }

            _currentEncounterIndex++;
            _encounterSelectionUI.ShowNextSelections(nextEncounters);
        }
    }
}
