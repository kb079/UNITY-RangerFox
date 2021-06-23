using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMusicaColinas : MonoBehaviour
{

    public int soundIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        //TODO: no esta bien ponerlo aqui
        if (c.gameObject.CompareTag("Player"))
        {
            MusicController audio = GameObject.FindGameObjectWithTag("Musica").GetComponent<MusicController>();
            audio.setAudioSource(soundIndex);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        //TODO: no esta bien ponerlo aqui
        if (c.gameObject.CompareTag("Player"))
        {
            MusicController audio = GameObject.FindGameObjectWithTag("Musica").GetComponent<MusicController>();
            audio.setAudioSource(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
