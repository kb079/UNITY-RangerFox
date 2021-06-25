using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMadriguera : Player
{
    void Update()
    {
        if (!isPaused)
        {
            playerMoves();
            activateActions();
        }

        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
            playerCamera.GetComponent<CameraManager>().isPaused = true;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("madriguera"))
        {
            SceneManager.LoadSceneAsync(2);
        }
    }

    private void OnTriggerEnter(Collider c) {}
}
