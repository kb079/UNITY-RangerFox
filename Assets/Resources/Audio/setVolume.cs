using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class setVolume : MonoBehaviour
{
    public AudioMixer mixer;
    // Start is called before the first frame update
    public void setLevel(float level)
    {
        mixer.SetFloat("SoundVolum", Mathf.Log10(level) * 20);
        /*GameConstants.volume = (int)(100*level);
        Debug.Log(level);*/
    }
}
