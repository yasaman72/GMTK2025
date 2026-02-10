using TMPro;
using UnityEngine;

namespace Deviloop
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Animator _startGameAnimator;
        [SerializeField] private TextMeshProUGUI _currentSeedText;

        private void Start()
        {
            _currentSeedText.text = "Current Seed: " + SeededRandom.GetSeed();
        }

        public void OnStart()
        {
            _startGameAnimator.SetTrigger("StartGame");
        }

        public void OnCloseGame()
        {
            Application.Quit();
            Debug.Log("Game Close");
        }

        public void OpenItchPage()
        {
            string itchUrl = "https://nasstaran.itch.io/deviloop";
            Application.OpenURL(itchUrl);
        }

        public void OnStartGameAnimationFinished()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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
                _currentSeedText.text = "Current Seed: " + SeededRandom.GetSeed();
            }
            else
            {
                Debug.LogWarning("Invalid seed value entered: " + seedValue);
            }
        }
    }
}