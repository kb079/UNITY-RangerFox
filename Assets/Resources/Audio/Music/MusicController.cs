using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController instance;

    public AudioSource audiosource;
    [SerializeField] AudioClip[] sonidos;
    private string actual;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        actual = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public void setAudioSource(int i)
    {
        audiosource.Stop();
        audiosource.clip = (sonidos[i]);
        audiosource.loop = true;
        audiosource.Play();
    }

    void Update()
    {
        if (!audiosource.isPlaying)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
            {
                setAudioSource(0);
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Madriguera")
            {
                setAudioSource(2);
               
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FinalMap")
            {
                setAudioSource(1);
              
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
            {
                audiosource.Stop();
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FinalBoss")
            {
                setAudioSource(4);
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
