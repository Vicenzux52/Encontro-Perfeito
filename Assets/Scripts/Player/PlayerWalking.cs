using UnityEngine;

public class PlayerWalking : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed, maxSpeed, drag;
    private bool foward, backward, left, right;

    //public float minX = -10f;
    //public float minZ = -10f;

    //public float maxX = 10f;
    //public float maxZ = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
        LimitVelocity();
        HandleDrag();

        if (foward)
        {
            rb.AddForce(moveSpeed * Vector3.forward * Time.deltaTime);
            foward = false;
        }

        if (backward)
        {
            rb.AddForce(moveSpeed * Vector3.back * Time.deltaTime);
            backward = false;
        }

        if (right)
        {
            rb.AddForce(moveSpeed * Vector3.right * Time.deltaTime);
            right = false;
        }

        if (left)
        {
            rb.AddForce(moveSpeed * Vector3.left * Time.deltaTime);
            left = false;
        }
    }

    //public void LateUpdate()
    //{
    //    Vector3 clampedPosition = transform.position;
    //    clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
    //    clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
    //    transform.position = clampedPosition;
    //}

    void Movement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            foward = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            backward = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            left = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            right = true;
        }

    }

    void LimitVelocity()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 limitation = horizontalVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitation.x, rb.linearVelocity.y, limitation.z);
        }
    }

    void HandleDrag()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z) / (1 + drag / 100) + new Vector3(0, rb.linearVelocity.y, 0);
    }

}
