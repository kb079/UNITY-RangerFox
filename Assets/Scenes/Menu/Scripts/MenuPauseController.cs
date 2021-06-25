using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseController : MonoBehaviour
{
    public void returnGame()
    {
        Cursor.visible = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().resumeGame();
    }
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void openSettings()
    {
       
        SceneManager.LoadSceneAsync("SettingsMenu", LoadSceneMode.Additive);
    }
    public void closeMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
