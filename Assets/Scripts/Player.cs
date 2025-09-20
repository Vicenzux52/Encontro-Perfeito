using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Rotas")]
    public float routeDistance = 1f;
    int routeQuantity = 1;
    [HideInInspector] public int route = 0;

    [Header("Movimento")]
    public float frontSpeed = 0.1f;
    public float lateralSpeed = 1;
    public float limitSpeed = 100;

    [Header("Pulos")]
    public float peakHeight = 5f;
    public float peakTime = 2f;
    public float jumpCooldown = 0;
    public bool isJumping = false;

    [Header("Delay")]
    public float delayForce = 30f;

    [Header("Slide")]
    bool isSliding = false;
    bool returnSlide = false;
    public float slideAngle = 75f;
    public float slideSpeed = 5f;
    public float slideTime = 1f;
    float slideTimer = 0f;
    public float slideShakeForce = 3f;
    public float slideShakeSpeed = 100f;

    //Outros
    Rigidbody rb;
    Transform orientation;

    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).name == "Orientation") orientation = transform.GetChild(i);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        rb.linearVelocity = Vector3.up * rb.linearVelocity.y + Vector3.forward * limitSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && route > -routeQuantity)
        {
            route--;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && route < routeQuantity)
        {
            route++;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && !isSliding)
        {
            isSliding = true;
        }
        if (isSliding)
        {
            Slide();
        }

        FrontalMovement();
        SideDash();
    }

    void FixedUpdate()
    {
        rb.AddForce(-orientation.up * CalculateGravity(), ForceMode.Acceleration);
        if (isJumping)
        {
            jumpCooldown += Time.deltaTime;
        }
        if (jumpCooldown >= 2 * peakTime)
        {
            isJumping = false;
            jumpCooldown = 0;
        }
    }

    void FrontalMovement()
    {
        rb.AddForce(orientation.forward * 0.01f * frontSpeed, ForceMode.VelocityChange);
        if (rb.linearVelocity.z > limitSpeed) rb.linearVelocity = Vector3.up * rb.linearVelocity.y + Vector3.forward * limitSpeed;
    }

    void SideDash()
    {
        route = Mathf.Clamp(route, -routeQuantity, routeQuantity);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(route * routeDistance,
        transform.position.y, transform.position.z), lateralSpeed * Time.deltaTime);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, peakTime * CalculateGravity(), rb.linearVelocity.z);
    }

    void Slide()
    {
        if (!returnSlide)
        {
            if (slideTimer == 0f) transform.RotateAround(transform.position, Vector3.right, -slideAngle * slideSpeed * Time.deltaTime);
            if ((transform.eulerAngles.x <= 361 - slideAngle && transform.eulerAngles.x >= 359 - slideAngle) || slideTimer != 0f)
            {
                slideTimer += Time.deltaTime;
                if (slideTimer > slideTime)
                {
                    slideTimer = 0f;
                    returnSlide = true;
                }
                transform.localRotation = Quaternion.Euler(new Vector3(-75 + Mathf.Sin(Time.time * slideShakeSpeed) * slideShakeForce, 0, 0));
            }
        }
        if (returnSlide)
        {
            transform.RotateAround(transform.position, Vector3.right, slideAngle * 0.75f * slideSpeed * Time.deltaTime);
            if (transform.eulerAngles.x > 0 && transform.eulerAngles.x < 5)
            {
                returnSlide = false;
                isSliding = false;
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }
        }
    }

    float CalculateGravity()
    {
        return 2 * peakHeight / (peakTime * peakTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 normal = contact.normal;
            if (Vector3.Dot(transform.forward, -normal) > 0.7f)
            {
                rb.linearVelocity = -orientation.forward * delayForce;
            }
            else if (Vector3.Dot(transform.forward, -normal) < 0.3f)
            {
                if (transform.position.x < collision.transform.position.x) route--;
                else route++;
            }
        }
    }
}
