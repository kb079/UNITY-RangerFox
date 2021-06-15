using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public void playStart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
