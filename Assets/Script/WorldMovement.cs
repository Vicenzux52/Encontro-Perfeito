using UnityEngine;

public class WorldMovement : MonoBehaviour
{
    public float worldSpeed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, -worldSpeed);
    }
}
