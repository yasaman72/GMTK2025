using System;
using UnityEngine;

namespace Deviloop
{
    public class PassageManager : MonoBehaviour, IInitiatable
    {
        public static Action<F_PassageOptionType> OnPassageOpenEvent;
        public static Action OnEncounterFinished;

        [SerializeField] private PassageSetup[] _passageSetups;

        [System.Serializable]
        public struct PassageSetup
        {
            public GameObject relatedObject;
            public PassageOptionType optionType;
        }

        public void Initiate()
        {
            OnPassageOpenEvent += OpenPassage;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnPassageOpenEvent -= OpenPassage;
        }

        private void OpenPassage(F_PassageOptionType config)
        {
            gameObject.SetActive(true);

            foreach (var passage in _passageSetups)
            {
                passage.relatedObject.SetActive(CompareType(config, passage.optionType));
            }
        }

        public void ClosePassage()
        {
            OnEncounterFinished?.Invoke();
            gameObject.SetActive(false);
        }

        private bool CompareType(F_PassageOptionType flag, PassageOptionType type)
        {
            var enumToflag = ToFlag(type);
            return (flag & enumToflag) == enumToflag;
        }
        private F_PassageOptionType ToFlag(PassageOptionType type)
        {
            System.Enum.TryParse(type.ToString(), out F_PassageOptionType flag);
            return flag;
        }
    }
}
