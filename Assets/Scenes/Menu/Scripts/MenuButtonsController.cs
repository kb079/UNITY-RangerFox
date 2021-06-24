using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButtonsController : MonoBehaviour
{
    
    public AudioSource Audio;
    public void playStart()
    {
        
        DontDestroyOnLoad(Audio.transform.gameObject);
        SceneManager.LoadSceneAsync("Tutorial");
    }
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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

    public void playAgain()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Madriguera");
    }
}
