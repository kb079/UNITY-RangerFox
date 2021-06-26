using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    private bool isOpened;
    private InventoryObject inventory;
    private Animator animator;
    public AudioClip used;

    void Start()
    {
        inventory  = GameObject.FindGameObjectWithTag("UIManager").GetComponent<InventoryObject>();
        isOpened = false;
        animator = GetComponent<Animator>();
    }

    public void openChest()
    {
        if (!isOpened)
        {

            GetComponent<AudioSource>().PlayOneShot(used);
            animator.SetBool("boton", true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().setHudText("");
            isOpened = true;
            StartCoroutine(giveObject(0.72f));
        }
    }

    IEnumerator giveObject(float time)
    {
        yield return new WaitForSeconds(time);

        int item = Random.Range(0, 2);
        if (SceneManager.GetActiveScene().name == "Tutorial") item = 1;
        inventory.addItem(inventory.itemTypes[item], 1);
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(GameConstants.key_interact)) openChest();
        }
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player") && !isOpened)
        {
            c.GetComponent<Player>().setHudText("Press [" + GameConstants.key_interact.ToString() + "] to open chest");
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player")) {
            Player p = c.GetComponent<Player>();
            if (p != null) p.setHudText("");
        }
    }
}
