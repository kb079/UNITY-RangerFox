using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 rotateValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse Y");
        float mouseY = Input.GetAxis("Mouse X");

        if (transform.rotation.y > -100 && transform.rotation.y < 100) { 
            rotateValue = new Vector3(mouseX, mouseY * -1, 0);
            transform.eulerAngles = transform.eulerAngles - rotateValue;
        }
    }
}
