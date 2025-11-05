using Unity.VisualScripting;
using UnityEngine;

public class TurningArea : MonoBehaviour
{
    GameObject cameraHolder;
    public int state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraHolder = Camera.main.transform.parent.gameObject;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name);
            cameraHolder.GetComponent<CameraHolder>().Turning(state);
        }
    }
}
