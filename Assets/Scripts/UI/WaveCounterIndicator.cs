using UnityEngine;

namespace Deviloop
{
    public class WaveCounterIndicator : MonoBehaviour
    {
        [SerializeField] private WaveIcon[] _waveIcons;

        private void Start()
        {
            OnWaveEncounterFinished();
        }

        private void OnEnable()
        {
            EnemyWaveEncounter.OnWaveEncounterStarted += OnWaveEncounterStarted;
            EnemyWaveEncounter.OnWaveEncounterFinished += OnWaveEncounterFinished;
            EnemyWaveEncounter.OnWaveEncounterProgressed += OnWaveEncounterProgressed;
        }

        private void OnDisable()
        {
            EnemyWaveEncounter.OnWaveEncounterStarted -= OnWaveEncounterStarted;
            EnemyWaveEncounter.OnWaveEncounterFinished -= OnWaveEncounterFinished;
            EnemyWaveEncounter.OnWaveEncounterProgressed -= OnWaveEncounterProgressed;
        }

        private void OnWaveEncounterStarted(int encountersCuount)
        {
            for (int i = 0; i < encountersCuount; i++)
                _waveIcons[i].gameObject.SetActive(true);
            _waveIcons[0].OnPass();

            for (int i = encountersCuount; i < _waveIcons.Length; i++)
                _waveIcons[i].gameObject.SetActive(false);
        }
        private void OnWaveEncounterProgressed(int currentIndex)
        {
            _waveIcons[currentIndex].OnPass();
        }

        private void OnWaveEncounterFinished()
        {
            for (int i = 0; i < _waveIcons.Length; i++)
                _waveIcons[i].gameObject.SetActive(false);
        }
    }
}
