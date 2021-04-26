using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    private Vector3 rotateValue;

    public Camera camera1;
    public Camera camera2;
    public GameObject crosshair;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* float mouseX = Input.GetAxis("Mouse Y");
         float mouseY = Input.GetAxis("Mouse X");

         if (transform.rotation.y > -100 && transform.rotation.y < 100) { 
             rotateValue = new Vector3(mouseX, mouseY * -1, 0);
             transform.eulerAngles = transform.eulerAngles - rotateValue;
         }*/
        if (Input.GetKey(KeyCode.K) )
        {
            ShowOverheadView();
        }
        else
        {
            ShowFirstPersonView();
        }

    }

    public void ShowOverheadView()
    {
        camera1.enabled = false;
        camera2.enabled = true;
        crosshair.SetActive(true);
    }

    public void ShowFirstPersonView()
    {
        camera1.enabled = true;
        camera2.enabled = false;
        crosshair.SetActive(false);
    }


}
