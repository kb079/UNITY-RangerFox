using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float maxY = 25;
    private float minY = -10;

    //Zoom camera
    public Camera camera1;
    public Camera camera2;
    public GameObject crosshair;

    private GameObject player;
    public bool isPaused = false;
    private Quaternion camRotation;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!isPaused && !player.GetComponent<Player>().isDead) {

            camRotation.x += Input.GetAxis("Mouse Y") * GameConstants.camMovementSpeed * (-1);
            camRotation.x = Mathf.Clamp(camRotation.x, minY, maxY);

            transform.localRotation = Quaternion.Euler(camRotation.x, 0, camRotation.z);

            float mouseY = Input.GetAxis("Mouse Y") * GameConstants.camMovementSpeed;
            transform.Rotate(-mouseY, 0, 0);

            //Zoom camera
            if (Input.GetKeyUp(GameConstants.key_cameraZoom)) ShowFirstPersonView();
            else if (Input.GetKeyDown(GameConstants.key_cameraZoom)) ShowOverheadView();
        }
    }

    private void ShowOverheadView()
    {
        camera1.enabled = false;
        camera2.enabled = true;
        crosshair.SetActive(true);
        Cursor.visible = false;
        player.GetComponent<Animator>().SetBool("crosshair", true);
    }

    private void ShowFirstPersonView()
    {
        player.GetComponent<Animator>().SetBool("crosshair", false);
        camera1.enabled = true;
        camera2.enabled = false;
        crosshair.SetActive(false);
        Cursor.visible = true;
    }
}