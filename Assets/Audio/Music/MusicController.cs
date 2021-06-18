using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audiosource;
    [SerializeField] AudioClip[] sonidos;
    private string actual;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        actual = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audiosource.isPlaying)
        {


            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
            {
                audiosource.PlayOneShot(sonidos[0]);
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Madriguera")
            {
                audiosource.PlayOneShot(sonidos[2]);
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FinalMap")
            {

                audiosource.PlayOneShot(sonidos[1]);
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
            {
                audiosource.Stop();
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FinalBoss")
            {
                audiosource.PlayOneShot(sonidos[3]);
            }
        }
        else
        {
            if (actual != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
            {
                actual = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                audiosource.Stop();
            }
        }
    }
}
