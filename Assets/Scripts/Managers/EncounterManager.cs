using UnityEngine;
using System.Collections.Generic;
using System;

namespace Deviloop
{
    public class EncounterManager : MonoBehaviour
    {
        // TODO: This should be in AreaManager
        public AreaData AllAreas;
        public EncounterSelectionUI EncounterSelection_UI;

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
            if (_currentAreaIndex >= AllAreas.Areas.Count)
            {
                Debug.Log("All areas completed. Game finished!");
                return;
            }
            
            CurrentArea = AllAreas.Areas[_currentAreaIndex];

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
            }
            else if (_currentEncounterIndex == CurrentArea.MaxEncounters)
            {
                Debug.Log("All encounters completed in area.");
                StartNextArea();
                return;
            }
            else 
            {
                ShowEncounterSelectionUI();
            }
        }

        public void ShowEncounterSelectionUI()
        {
            List<BaseEncounter> nextEncounters = new List<BaseEncounter>();

            if (_currentEncounterIndex == CurrentArea.MaxEncounters - 3)
            {
                Debug.Log("Starting a combat before pre boss shop encounter.");

                nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>());
                nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>());
            }
            else if (_currentEncounterIndex == CurrentArea.MaxEncounters - 2)
            {
                Debug.Log("Starting pre boss shop/rest encounter.");

                nextEncounters.Add(CurrentArea.GetRandomEncounterType<ShopEncounter>());
                //nextEncounters.Add(CurrentArea.GetRandomEncounterType<RestEncounter>());
            }
            else if (_currentEncounterIndex == CurrentArea.MaxEncounters - 1)
            {
                Debug.Log("Starting boss encounter.");
                nextEncounters.Add(CurrentArea.BossEncounter);
            }
            else
            {
                if (CurrentEncounter is ShopEncounter)
                {
                    nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>());
                    nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>());
                }
                else
                {
                    BaseEncounter firstEncounter = CurrentArea.GetRandomEncounter();
                    nextEncounters.Add(firstEncounter);

                    if(firstEncounter is ShopEncounter)
                    {
                        nextEncounters.Add(CurrentArea.GetRandomEncounterType<CombatEncounter>());
                    }
                    else
                    {
                        nextEncounters.Add(CurrentArea.GetRandomEncounter());
                    }
                }

                Debug.Log($"Starting encounter {_currentEncounterIndex + 1}/{CurrentArea.MaxEncounters} in area {CurrentArea.AreaName}: {CurrentEncounter.name}");
            }

            _currentEncounterIndex++;
            EncounterSelection_UI.ShowNextSelections(nextEncounters);
        }
    }
}
