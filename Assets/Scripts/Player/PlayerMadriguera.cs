using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMadriguera : Player
{
    void Update()
    {
        playerMoves();
        activateActions();
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("madriguera"))
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

    private void OnTriggerEnter(Collider c) {}
}
