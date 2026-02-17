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
            EnemyWaveData.OnWaveEncounterStarted += OnWaveEncounterStarted;
            EnemyWaveData.OnWaveEncounterFinished += OnWaveEncounterFinished;
            EnemyWaveData.OnWaveEncounterProgressed += OnWaveEncounterProgressed;
        }

        private void OnDisable()
        {
            EnemyWaveData.OnWaveEncounterStarted -= OnWaveEncounterStarted;
            EnemyWaveData.OnWaveEncounterFinished -= OnWaveEncounterFinished;
            EnemyWaveData.OnWaveEncounterProgressed -= OnWaveEncounterProgressed;
        }

        private void OnWaveEncounterStarted(int encountersCuount)
        {
            for (int i = 0; i < encountersCuount; i++)
                _waveIcons[i].gameObject.SetActive(true);

            for (int i = encountersCuount; i < _waveIcons.Length; i++)
                _waveIcons[i].gameObject.SetActive(false);
        }
        private void OnWaveEncounterProgressed(int currentIndex)
        {
            _waveIcons[currentIndex - 1].OnPass();
        }

        private void OnWaveEncounterFinished()
        {
            for (int i = 0; i < _waveIcons.Length; i++)
                _waveIcons[i].gameObject.SetActive(false);
        }
    }
}
