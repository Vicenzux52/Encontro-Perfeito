using System;
using UnityEngine;

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
    bool onMaxSpeed = false;

    [Header("Pulos")]
    public float peakHeight = 5f;
    public float peakTime = 2f;
    public float jumpCooldown = 0;
    public bool isJumping = false;

    [Header("Delay")]
    public float delayForce = 30f;
    bool isDelayed = false;
    public float lateralSpeedDelay = 0.5f;
    public float delayTime = 1;
    float delayCounter = 0;

    [Header("Slide")]
    public float slideAngle = 75f;
    public float slideSpeed = 5f;
    public float slideTime = 1f;
    bool isSliding = false;
    bool returnSlide = false;
    float slideTimer = 0f;
    public float slideShakeForce = 3f;
    public float slideShakeSpeed = 100f;

    [Header("Hit")]
    public Material hitMaterial;
    private Material originalMaterial;
    private Renderer rend;
    public float hitDuration = 3f;
    private bool isHit = false;

    //Outros
    Rigidbody rb;
    Transform orientation;

    public AudioSource audioSource;
    public AudioSource collectibleSound;

    //Mobile
    private Vector2 startTouch;
    private float lastTapTime = 0f;
    private int tapCount = 0;


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
        collectibleSound = transform.Find("CollectibleAudio").GetComponent<AudioSource>();


        rb.linearVelocity = Vector3.up * rb.linearVelocity.y + Vector3.forward * limitSpeed;

        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            DetectSwipes();
        }
        else 
        {
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && route > -routeQuantity)
            {
                route--;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }

            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && route < routeQuantity)
            {
                route++;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

                Jump();
                isJumping = true;
            }

            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding && !isJumping)
            {
                isSliding = true;
            }
        }

        if (isSliding)
        {
            Slide();
        }

        FrontalMovement();
        SideDash();

        if (isDelayed)
        {
            Delay();
        }
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
        if (!onMaxSpeed) rb.AddForce(orientation.forward * 0.01f * frontSpeed, ForceMode.VelocityChange);
        else rb.linearVelocity = Vector3.up * rb.linearVelocity.y + Vector3.forward * limitSpeed;
        if (rb.linearVelocity.z > limitSpeed)
        {
            rb.linearVelocity = Vector3.up * rb.linearVelocity.y + Vector3.forward * limitSpeed;
            onMaxSpeed = true;
        }
        if (isDelayed) rb.linearVelocity = Vector3.up * rb.linearVelocity.y + Vector3.forward * limitSpeed * lateralSpeedDelay;
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

    void Delay()
    {
        if (delayCounter < delayTime) delayCounter += Time.time;
        else isDelayed = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 normal = contact.normal;
            if ((Vector3.Dot(transform.forward, -normal) > 0.7f || isSliding) && Vector3.Dot(-transform.up, -normal) < 0.7f) //bateu de frente ou deslizando
            {
                onMaxSpeed = false;
                rb.linearVelocity = -orientation.forward * delayForce;
            }
            else if (Vector3.Dot(transform.forward, -normal) < 0.3f) //bateu de lado
            {
                if (transform.position.x < collision.transform.position.x) route--;
                else route++;
                onMaxSpeed = false;
                isDelayed = true;
            }

            if (!isHit)
            {
                StartCoroutine(FlashMaterial());
            }

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            collectibleSound.Play();
            Destroy(other.gameObject);
        }
    }

    private System.Collections.IEnumerator FlashMaterial()
    {
        isHit = true;
        rend.material = hitMaterial;
        yield return new WaitForSeconds(hitDuration);
        rend.material = originalMaterial;
        isHit = false;
    }

    void DetectSwipes()
    {
        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
        {
            startTouch = t.position;
        }
        else if (t.phase == TouchPhase.Ended)
        {
            Vector2 delta = t.position - startTouch;

            if (delta.magnitude > 100)
            {
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    if (delta.x > 0)
                    {
                        Debug.Log("Swipe Right");

                        route++;
                        if (!audioSource.isPlaying)
                        {
                            audioSource.Play();
                        }

                    }
                    else
                    {
                        Debug.Log("Swipe Left");

                        route--;
                        if (!audioSource.isPlaying)
                        {
                            audioSource.Play();
                        }
                    }
                }
                else
                {
                    if (delta.y > 0 && !isJumping)
                    {
                        Debug.Log("Swipe Up");

                        if (!audioSource.isPlaying)
                        {
                            audioSource.Play();
                        }

                        Jump();
                        isJumping = true;
                    }
                    else if (delta.y < 0 && !isSliding)
                    {
                        Debug.Log("Swipe Down");

                        isSliding = true;
                    }
                }
            }
        }

        //if (Input.touchCount == 1)
        //{

        //}
    }
}
