using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField] private bool _shouldLog = true;

        [Tooltip("Enable endless mode for testing purposes.")]
        [SerializeField] private bool _endlessMode = true;

        // TODO: This should be in AreaManager
        [SerializeField] private AreaData _allAreas;
        [SerializeField] private EncounterSelectionUI _encounterSelectionUI;

        public static Area CurrentArea;
        private static int _currentAreaIndex = -1;

        private static int _currentEncounterIndex = 0;
        public static int CurrentEncounterIndex => _currentEncounterIndex;
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
            _currentEncounterIndex = 0;
            StartNextArea();
            _allAreas.Setup();
        }

        public void StartNextArea()
        {
            _currentAreaIndex++;


            if (!_endlessMode && _currentAreaIndex >= _allAreas.Areas.Count)
            {
                Logger.Log("All areas completed. Game finished.", _shouldLog);
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
                Logger.Log($"Starting encounters in area: {CurrentArea.AreaName}", _shouldLog);

                CurrentEncounter = CurrentArea.GetRandomEncounterType<CombatEncounter>(true);
                _currentEncounterIndex++;
                CurrentEncounter.StartEncounter();
                return;
            }
            
            if (_currentEncounterIndex == CurrentArea.MaxEncounters)
            {
                // TODO: put this endless somewhere better
                if (_endlessMode)
                {
                    Logger.Log("Endless mode enabled - restarting area.", _shouldLog);
                    _currentEncounterIndex = 0;
                    EncounterFinished();
                    return;
                }
                Logger.Log("All encounters completed in area.", _shouldLog);
                StartNextArea();
                return;
            }

            _currentEncounterIndex++;
            ShowEncounterSelectionUI(DeterminNextEncounter());
        }

        public List<BaseEncounter> DeterminNextEncounter()
        {
            List<BaseEncounter> nextEncounters = new List<BaseEncounter>();

            if (!_endlessMode && _currentEncounterIndex == CurrentArea.MaxEncounters - 3)
            {
                Logger.Log("Starting a combat before pre boss shop encounter.", _shouldLog);

                // Avoiding having the same encounter
                nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>(false));
                nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>(false, new List<BaseEncounter>() { nextEncounters[0] }));
            }
            else if (!_endlessMode && _currentEncounterIndex == CurrentArea.MaxEncounters - 2)
            {
                Logger.Log("Starting pre boss shop/rest encounter.", _shouldLog);

                nextEncounters.Add(CurrentArea.GetRandomEncounterType<ShopEncounter>(false));
                //nextEncounters.Add(CurrentArea.GetRandomEncounterType<RestEncounter>());
            }
            else if (!_endlessMode && _currentEncounterIndex == CurrentArea.MaxEncounters - 1)
            {
                Logger.Log("Starting boss encounter.", _shouldLog);
                nextEncounters.Add(CurrentArea.BossEncounter);
            }
            else
            {
                List<BaseEncounter> skippingEncounters = new List<BaseEncounter>();

                // Don't allow two shops in a row
                if (CurrentEncounter is ShopEncounter)
                    skippingEncounters.Add(CurrentEncounter);

                nextEncounters.Add(CurrentArea.GetRandomEncounter(false, skippingEncounters));
                skippingEncounters.Add(nextEncounters[0]);
                nextEncounters.Add(CurrentArea.GetRandomEncounter(false, skippingEncounters));

                Logger.Log($"Starting encounter {_currentEncounterIndex + 1}/{CurrentArea.MaxEncounters} in area {CurrentArea.AreaName}: {CurrentEncounter.name}", _shouldLog);
            }

            return nextEncounters;
        }

        public void ShowEncounterSelectionUI(List<BaseEncounter> nextEncounters)
        {
            _encounterSelectionUI.ShowNextSelections(nextEncounters);
        }
    }
}
