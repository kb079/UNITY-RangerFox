using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Vector3 posMadriguera;
    public Vector3 posFinalMap;
    public Vector3 posFinalBoss;
    public List<Material> skyboxes = new List<Material>();

    private Player player;
    private Camera[] playerCameras;

    void Start()
    {
        player = GetComponent<Player>();
        playerCameras = GetComponentsInChildren<Camera>();
        posMadriguera = new Vector3(-10, -7, -35);
        posFinalMap = new Vector3(16.9f, 0.13f, 15.71f);
        posFinalBoss = new Vector3(174.460007f, 10.1999998f, 121.268112f);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("madriguera"))
        {
            playerCameras[0].clearFlags = CameraClearFlags.Color;
            playerCameras[1].clearFlags = CameraClearFlags.Color;
            SceneManager.LoadSceneAsync("Madriguera");
            transform.position = posMadriguera;
        }

        if (c.gameObject.CompareTag("SceneBoss"))
        {
            SceneManager.LoadSceneAsync("FinalBoss");
            transform.position = posFinalBoss;
            player.isDead = true;
            player.anim.SetFloat("playerX", 0);
            player.anim.SetFloat("playerZ", 0);
            transform.eulerAngles = new Vector3(0, 90, 0);
            playerCameras[0].clearFlags = CameraClearFlags.Skybox;
            playerCameras[1].clearFlags = CameraClearFlags.Skybox;
            StartCoroutine(player.cor_EndCinematic(20f));
        }

        if (c.gameObject.CompareTag("madrigueraExit"))
        {
            SceneManager.LoadSceneAsync("FinalMap");
            playerCameras[0].clearFlags = CameraClearFlags.Skybox;
            playerCameras[1].clearFlags = CameraClearFlags.Skybox;
            //playerCameras[0].gameObject.GetComponent<Renderer>().material = skyboxes[0];
            //playerCameras[1].gameObject.GetComponent<Renderer>().material = skyboxes[0];

            transform.position = posFinalMap;
        }

    }
}
