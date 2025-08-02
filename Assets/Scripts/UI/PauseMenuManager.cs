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
}
