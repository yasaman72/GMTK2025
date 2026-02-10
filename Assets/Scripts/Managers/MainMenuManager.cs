using TMPro;
using UnityEngine;

namespace Deviloop
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Animator _startGameAnimator;

        public void OnStart()
        {
            _startGameAnimator.SetTrigger("StartGame");
            // TODO: better, more generic way to reset game state when starting a new game
            RelicManager.ClearAllRelics();
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
    }
}