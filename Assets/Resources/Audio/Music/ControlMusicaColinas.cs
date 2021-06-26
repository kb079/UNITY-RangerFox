using UnityEngine;

public class ControlMusicaColinas : MonoBehaviour
{
    public int soundIndex;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            MusicController audio = GameObject.FindGameObjectWithTag("Musica").GetComponent<MusicController>();
            audio.setAudioSource(soundIndex);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            MusicController audio = GameObject.FindGameObjectWithTag("Musica").GetComponent<MusicController>();
            audio.setAudioSource(1);
        }
    }
}
