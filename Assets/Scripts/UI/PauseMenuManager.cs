using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

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
        // reload the current scene
        Time.timeScale = 1f;
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
    }

    public void OnLanguageChange()
    {
        int currentLocaleIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        int localeCount = LocalizationSettings.AvailableLocales.Locales.Count;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[++currentLocaleIndex % localeCount];
    }
}
