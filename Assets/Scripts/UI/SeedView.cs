using TMPro;
using UnityEngine;

namespace Deviloop
{
    public class SeedView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentSeedText;

        private void Start()
        {
            UpdateSeedDisplay();
        }

        private void OnEnable()
        {
            UpdateSeedDisplay();
        }

        public void CopySeedToClipboard()
        {
            GUIUtility.systemCopyBuffer = SeededRandom.GetSeed().ToString();
        }
        public void OnSeedValueEdited(string seedValue)
        {
            if (int.TryParse(seedValue, out int seed))
            {
                SeededRandom.SetSeed(seed);
                UpdateSeedDisplay();
            }
            else
            {
                Debug.LogWarning("Invalid seed value entered: " + seedValue);
            }
        }

        private void UpdateSeedDisplay()
        {
            _currentSeedText.text = "Current Seed: " + SeededRandom.GetSeed();
        }
    }
}
