using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonsController : MonoBehaviour
{

    public AudioSource Audio;

    public GameObject buttons, panel;
    public Button newButton, resume, closePanel;
    public GameObject playerInst;
    public GameObject hudInst;

    public void playStart()
    {
        
        if (PlayerSavingData.checkExistsData())
        {
            toggleMenuOption();
        }
        else
        {
            SceneManager.LoadSceneAsync("Tutorial");
        }
    }

    public void toggleMenuOption()
    {
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);
            buttons.SetActive(true);
        }
        else
        {
            panel.SetActive(true);
            buttons.SetActive(false);

            newButton.onClick.AddListener(newGame);
            resume.onClick.AddListener(resumeGame);
        }
    }

    private void newGame()
    {
        PlayerSavingData.deleteData();
        SceneManager.LoadSceneAsync("Tutorial");
    }

    private void resumeGame()
    {
        SceneManager.LoadSceneAsync("Madriguera");
        Instantiate(hudInst);
        Instantiate(playerInst, new Vector3(-10, -7, -35), Quaternion.identity);
        PlayerSavingData.runLoadData = true;
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

        Time.timeScale = 1;
        Player.getInstance().resetGame();
        //Instantiate(HUDController.getInstance().gameObject);
        //Instantiate(Player.getInstance().gameObject, new Vector3(-10, -7, -35), Quaternion.identity);
    }
    
}
