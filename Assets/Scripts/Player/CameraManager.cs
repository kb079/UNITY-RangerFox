using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Cam Rotation Values from transform.rotation.x
    private float maxY = -0.12f;
    private float minY = 0.40f;
    private bool canRotate, touchingUp, touchingDown;

    //Zoom camera
    public Camera camera1;
    public Camera camera2;
    public GameObject crosshair;
    private bool blocked;

    void Update()
    {
        //Camera limit
        touchingUp = (Input.mousePosition.y >= Screen.height * 0.95) ? true : false;
        touchingDown = (Input.mousePosition.y <= Screen.height / 2) ? true : false;
        canRotate = true;

        float camRotation = gameObject.transform.rotation.x;

        if (camRotation <= maxY && touchingUp || camRotation >= minY && touchingDown) canRotate = false;

        if (canRotate)
        {
            float mouseY = Input.GetAxis("Mouse Y") * GameConstants.camMovementSpeed;
            transform.Rotate(-mouseY, 0, 0);
        }

        //Zoom camera
        if (Input.GetKey(GameConstants.key_cameraZoom) && !blocked) StartCoroutine(doAction());
    }

    IEnumerator doAction()
    {
        blocked = true;
        if (!crosshair.activeInHierarchy)
        {
            ShowOverheadView();
        }
        else
        {
            ShowFirstPersonView();
        }
        yield return new WaitForSeconds(0.5f);
        blocked = false;
    }

    private void ShowOverheadView()
    {
        camera1.enabled = false;
        camera2.enabled = true;
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    private void ShowFirstPersonView()
    {
        camera1.enabled = true;
        camera2.enabled = false;
        crosshair.SetActive(false);
        Cursor.visible = true;
    }
}
