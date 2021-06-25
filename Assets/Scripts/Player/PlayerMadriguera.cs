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

        #if UNITY_WEBGL
                if (Input.GetKeyDown(KeyCode.P) && !isPaused) pauseGame();
        #endif

        #if UNITY_STANDALONE
                if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) pauseGame();
        #endif
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("madriguera"))
        {
            SceneManager.LoadSceneAsync("FinalMap");
        }
    }

    private void OnTriggerEnter(Collider c) {}
}
