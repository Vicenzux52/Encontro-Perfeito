using System;
using UnityEngine;

public class NewPlayer : MonoBehaviour
{
    [Header("Rotas")]
    public float routeDistance = 1f;
    public int routeQuantity = 1;
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
    Vector3 initialCenter;
    float initialHeight;
    public Vector3 slideCenter;
    public float slideHeight;

    [Header("Hit")]
    public Material hitMaterial;
    private Material originalMaterial;
    private Renderer rend;
    public float hitDuration = 3f;
    private bool isHit = false;

    //Outros
    CharacterController cC;
    Vector3 velocity;
    Transform orientation;

    public AudioSource audioSource;
    public AudioSource collectibleSound;



    void Start()
    {
        cC = GetComponent<CharacterController>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Orientation")orientation = transform.GetChild(i);
            if (transform.GetChild(i).CompareTag("Model")) rend = transform.GetChild(i).GetComponent<Renderer>();
        }
                
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        collectibleSound = transform.Find("CollectibleAudio").GetComponent<AudioSource>();

        initialCenter = cC.center;
        initialHeight = cC.height;
        
        velocity.z = limitSpeed;
    }

    void Update()
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

/*
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
        }*/

        //if (isSliding) Slide();

        FrontalMovement();
        SideDash();

        //if (isDelayed) Delay();

        if (cC.isGrounded) isJumping = true;

        else isJumping = false;
    }

    void FixedUpdate()
    {
        velocity.y -= CalculateGravity() * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);
    }

    void FrontalMovement()
    {
        velocity.z = Mathf.Min(velocity.z + frontSpeed/10 * Time.deltaTime, limitSpeed/10);
        if (isDelayed)
            velocity.z = limitSpeed * lateralSpeedDelay;
    }

    void SideDash()
    {
        route = Mathf.Clamp(route, -routeQuantity, routeQuantity);
        if (transform.position.x == route * routeDistance) velocity.x = 0;
        else velocity.x = route - transform.position.x * lateralSpeed;
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(route * routeDistance,
        //transform.position.y, transform.position.z), lateralSpeed * Time.deltaTime);
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(2 * CalculateGravity() * peakHeight);
        isJumping = true;
    }

    void Slide()
    {
        if (!returnSlide)
        {
            cC.height = slideHeight;
            cC.center = slideCenter;
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
                cC.height = initialHeight;
                cC.center = initialCenter;
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
                velocity.z = -delayForce;
            }
            else if (Vector3.Dot(transform.forward, -normal) < 0.3f) //bateu de lado
            {
                if (transform.position.x < collision.transform.position.x) route--;
                else route++;
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
}
