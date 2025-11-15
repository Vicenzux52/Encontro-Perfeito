using UnityEngine;

public class FloatY : MonoBehaviour
{
    public float minY = -0.5f;
    public float maxY = 1f;
    public float speed = 2f;

    private float startY;
    private float heightDifference;

    void Start()
    {
        startY = transform.position.y;

        heightDifference = maxY - minY;
    }

    void Update()
    {
        float newY = minY + Mathf.PingPong(Time.time * speed, heightDifference);
        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }
}
