using UnityEngine;

public class PlayerWalking : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed, maxSpeed, drag;
    private Vector3 movementInput;
    private bool isMoving;
    public GameObject walkVfx;

    public FixedJoystick mobileJoystick;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        walkVfx.SetActive(false);

        if (mobileJoystick == null)
        {
            mobileJoystick = FindObjectOfType<FixedJoystick>();
        }
    }

    void Update()
    {
        GetMovementInput();
        RotateTowardsCamera();

        if (isMoving == false)
        {
            walkVfx.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 moveDirection = GetMovementDirectionRelativeToPlayer();
            rb.AddForce(moveDirection * moveSpeed);
            walkVfx.SetActive(true);
        }

        LimitVelocity();
        HandleDrag();
        isMoving = false;
    }

    void RotateTowardsCamera()
    {
        if (Camera.main == null) return;

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;

        if (cameraForward != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }
    }

    void GetMovementInput()
    {
        movementInput = Vector3.zero;

        bool hasJoystickInput = false;

        if (mobileJoystick != null && mobileJoystick.gameObject.activeInHierarchy)
        {
            float horizontal = mobileJoystick.Horizontal;
            float vertical = mobileJoystick.Vertical;

            if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            {
                movementInput.x = horizontal;
                movementInput.z = vertical;
                hasJoystickInput = true;
                isMoving = true;
            }
        }

        if (!hasJoystickInput)
        {
            if (Input.GetKey(KeyCode.W))
            {
                movementInput.z += 1;
                isMoving = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movementInput.z -= 1;
                isMoving = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movementInput.x -= 1;
                isMoving = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                movementInput.x += 1;
                isMoving = true;
            }
        }

        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }
    }

    Vector3 GetMovementDirectionRelativeToPlayer()
    {
        Vector3 forwardRelative = transform.forward * movementInput.z;
        Vector3 rightRelative = transform.right * movementInput.x;

        return (forwardRelative + rightRelative).normalized;
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