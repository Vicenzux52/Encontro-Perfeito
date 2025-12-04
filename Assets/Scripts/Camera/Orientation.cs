using UnityEngine;

public class Orientation : MonoBehaviour
{
    [Header("Eixos Travados")]
    public bool x = true;
    public float xDegrees;
    public bool y = false;
    public float yDegrees;
    public bool z = false;
    public float zDegrees;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xDegrees = transform.rotation.x;
        yDegrees = transform.rotation.y;
        zDegrees = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (x) transform.rotation = Quaternion.Euler(xDegrees, transform.rotation.y, transform.rotation.z);
        if (y) transform.rotation = Quaternion.Euler(transform.rotation.x, yDegrees, transform.rotation.z);
        if (z) transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zDegrees);
    }
}
