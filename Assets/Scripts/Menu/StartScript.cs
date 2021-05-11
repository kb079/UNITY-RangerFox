using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public void playStart()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
