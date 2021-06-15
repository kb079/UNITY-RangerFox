using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseController : MonoBehaviour
{
    public void returnGame()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().resumeGame();
    }
    public void openSettings()
    {
        SceneManager.LoadSceneAsync("SettingsMenu", LoadSceneMode.Additive);
    }
    public void closeMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
