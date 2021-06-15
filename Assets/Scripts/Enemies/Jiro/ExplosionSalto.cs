using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSalto : MonoBehaviour
{
    public GameObject player;
    private Rigidbody playerRB;
    private float scaleVal = 0.18f;
    private float baseDamage = 30;
    private float finalDamage = 0;
    private void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        Destroy(gameObject, 0.3f);
    }
    private void Update()
    {
        float scale = (float)(transform.localScale.x + scaleVal);
        // Puede llegar a restar unos 14 de daño del daño base
        finalDamage += scaleVal;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerRB.AddForce(Vector3.up * 7f, ForceMode.Impulse);
            transform.LookAt(player.transform);
            playerRB.AddForce(transform.forward * 10f, ForceMode.Impulse);

            finalDamage = baseDamage - finalDamage;
            // Por si se le quiere aumentar la máxima resta de daño
            if (finalDamage < 15) finalDamage = 15;
            Player p = other.gameObject.GetComponent<Player>();
            if (!p.isBarrierActive)
            {
                p.doDamage((int)finalDamage, 1);
            }
            else
            {
                p.useMana(finalDamage);
            }
            Destroy(gameObject);
        }
    }
}
