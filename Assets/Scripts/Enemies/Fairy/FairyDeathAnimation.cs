using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDeathAnimation : MonoBehaviour
{
    private Animation animacion;
    // Start is called before the first frame update
    void Start()
    {
        animacion = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("terreno"))
        {
            animacion.Stop();
        }
    }
}
