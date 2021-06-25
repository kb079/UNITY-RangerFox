using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FiinishGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(goToTitle(8f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator goToTitle(float end)
    {
        yield return new WaitForSeconds(end);
        SceneManager.LoadScene("MainMenu");
    }
}
