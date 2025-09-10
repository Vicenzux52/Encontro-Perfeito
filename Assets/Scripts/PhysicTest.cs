using UnityEngine;

public class PhysicTest : MonoBehaviour
{
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A pressed");
            transform.Translate(-1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D pressed");
            transform.Translate(1, 0, 0);
        }
    }
}