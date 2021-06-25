using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FiinishGame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(goToTitle(11f));
    }

    IEnumerator goToTitle(float end)
    {
        yield return new WaitForSeconds(end);
        SceneManager.LoadScene("MainMenu");
    }
}
