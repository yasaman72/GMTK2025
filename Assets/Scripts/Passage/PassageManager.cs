using System;
using UnityEngine;

namespace Deviloop
{
    public class PassageManager : MonoBehaviour, IInitiatable
    {
        public static Action OnPassageOpenEvent;
        public static Action OnEncounterFinished;

        public void Initiate()
        {
            OnPassageOpenEvent += OpenPassage;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnPassageOpenEvent -= OpenPassage;
        }

        private void OpenPassage()
        {
            gameObject.SetActive(true);
        }

        public void ClosePassage()
        {
            OnEncounterFinished?.Invoke();
            gameObject.SetActive(false);
        }

    }
}
