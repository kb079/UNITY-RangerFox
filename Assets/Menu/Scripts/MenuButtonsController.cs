using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButtonsController : MonoBehaviour
{
    
    public AudioSource Audio;
    public void playStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        DontDestroyOnLoad(Audio.transform.gameObject);
        SceneManager.LoadSceneAsync("Tutorial");
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
