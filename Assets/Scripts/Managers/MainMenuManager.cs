using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator _startGameAnimator;

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
}
