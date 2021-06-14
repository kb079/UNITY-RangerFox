using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonsController : MonoBehaviour
{
    public void playStart()
    {
        SceneManager.LoadSceneAsync("Madriguera");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void openSettings()
    {
        SceneManager.LoadSceneAsync("SettingsMenu", LoadSceneMode.Additive);
    }

    public void closeSettings()
    {
        SceneManager.UnloadSceneAsync("SettingsMenu");

    }
}
