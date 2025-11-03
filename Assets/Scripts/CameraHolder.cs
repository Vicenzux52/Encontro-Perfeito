using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraHolder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float cameraDistance = 7.5f;
    public float cameraHeight = 2;
    public float xRotation = 15;
    public float yRotation = 0;
    public float rotationSpeed = 60;
    public int cameraState = 0; //0 - Normal | 1 - Lateral
    public bool onTransition = false;
    Quaternion targetRotation;
    GameObject mainCamera;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.gameObject;
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        mainCamera.transform.localPosition = Vector3.right * mainCamera.transform.position.x;
        mainCamera.transform.localPosition += Vector3.up * cameraHeight + Vector3.back * cameraDistance;
        targetRotation = Quaternion.Euler(xRotation, 0, yRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        if (cameraState == 0 && transform.eulerAngles.y != 0) onTransition = true;
        else if (cameraState == 1 && transform.eulerAngles.y != -90) onTransition = true;
        else onTransition = false;
        if (cameraState == 0) targetRotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        if (cameraState == 1) targetRotation = Quaternion.Euler(transform.eulerAngles.x, -90, transform.eulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void Turning(int state)
    {
        cameraState = state;
    }
}
