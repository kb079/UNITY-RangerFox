using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioSource audiosource;
    public void playSound(AudioClip audio)
    {
        audiosource.PlayOneShot(audio);
    }
}
