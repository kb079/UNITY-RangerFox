using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    private static Sounds instance;

    public AudioSource audiosource;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    public void playSound(AudioClip audio)
    {
        if (audiosource != null && audio != null)
        {
            audiosource.PlayOneShot(audio);
        }
    }
}
