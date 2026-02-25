using UnityEngine;

namespace Deviloop
{
    public class WinScreen : UIView
    {
        public override void Open()
        {
            GenericInputBinder.IsGameplayInputBlocked = true;

            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }
        public override void Close()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }

        public void OnResume()
        {
            Time.timeScale = 1f;
            GenericInputBinder.IsGameplayInputBlocked = false;

            Close();
        }

        // TODO: this is a dirty implementation that uses the same code for pause, game over and win page
        public void OnCloseGame()
        {
            Application.Quit();
            Debug.Log("Game Closed");
        }

        public void OnGoToMainMenu()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void OnRestart()
        {
            // reload the current scene
            // TODO: better implementation that resets the game state without reloading the scene
            Time.timeScale = 1f;
            int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            RelicManager.ClearAllRelics();
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
