using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{

    public void OnResume()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void OnCloseGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void OpenItchPage()
    {
        string itchUrl = "https://nasstaran.itch.io/deviloop";
        Application.OpenURL(itchUrl);
    }

    public void OnGoToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // TODO: this is a dirty implementation that uses the same code for pause and game over page
    public void OnRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
