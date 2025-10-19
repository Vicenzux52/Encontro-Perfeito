using System;
<<<<<<< HEAD
using System.Diagnostics;
using Unity.Mathematics;
=======
>>>>>>> parent of 54caebd (Adicionado a virada de camera)
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    [Header("Rotas")]
    [SerializeField] float routeDistance = 1f;
    [SerializeField] int routeQuantity = 1;
    [HideInInspector] public int route = 0;

    [Header("Movimento")]
    public float frontSpeed = 0.1f;
    public float lateralSpeed = 1;
    public float limitSpeed = 100;
    bool onMaxSpeed = false;

    [Header("Pulos")]
    public float JumpHeight = 5f;
    public float jumpDuration = 2f;
    public float gravity = 10;
    float initialYJump = 0;
    float timeX = 0;
    bool isJumping = false;

    [Header("Delay")]
    public float delayForce = 30f;
    bool isDelayed = false;
    public float lateralSpeedDelay = 0.5f;
    public float delayTime = 1;
    float delayCounter = 0; //deve tirar

    [Header("Slide")]
    [SerializeField] float slideAngle = 75f;
    [SerializeField] float slideSpeed = 5f;
    [SerializeField] float slideTime = 1f;
    bool isSliding = false;
    bool returnSlide = false;
    float slideTimer = 0f; //deve tirar
    [SerializeField] float slideShakeForce = 3f;
    [SerializeField] float slideShakeSpeed = 100f;

    [Header("Hit")]
    public Material hitMaterial;
    private Material originalMaterial;
    private Renderer rend;
    public float hitDuration = 3f;
    private bool isHit = false;

    [Header("Hit")]
    public int upgrade;
    //Outros
    Rigidbody rb;
    Transform orientation;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource collectibleSound;


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

        //upgrade = upgrade selecionado
        SetUpgrade();

        rb.linearVelocity = Vector3.up * rb.linearVelocity.y + Vector3.forward * limitSpeed;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.CompareTag("PlayerModel"))
            {
                rend = transform.GetChild(i).GetComponent<Renderer>();
                originalMaterial = rend.material;
            }
        }
        
    }

    void Update()
    {
        GetInputs();
        Slide();
        FrontalMovement();
        SideDash();
        Delay();
        Jump();
    }

    void FixedUpdate()
    {
        if (!isJumping) rb.AddForce(-orientation.up * gravity, ForceMode.Acceleration);
    }
    
    void GetInputs()
    {
        bool leftInputs = (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && route > -routeQuantity;
        bool rightInputs = (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && route < routeQuantity;
        bool upInputs = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && !isSliding && !isJumping; //Decidir se o pulo cancela o slide ou só pula mesmo
        bool slideInputs = (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding && !isJumping;
        slideInputs |= (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) && !isSliding && !isJumping;
        bool downInputs = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && isJumping;

        if (leftInputs)
        {
            route--;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (rightInputs)
        {
            route++;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (upInputs)
        {
            initialYJump = transform.position.y;
            isJumping = true;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }

        if (slideInputs)
        {
            isSliding = true;
        }

        if (downInputs)
        {
            Jump();
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
        if (isJumping)
        {
            transform.position = new Vector3(transform.position.x, initialYJump + JumpHeight * Mathf.Pow(Mathf.Sin(timeX / jumpDuration), 1f / 2f),
            transform.position.z);
            timeX += Time.deltaTime;
            if (Mathf.Sin(timeX / jumpDuration) < 0)
            {
                isJumping = false;
                timeX = 0;
            }
        }
    }

    void Slide()
    {
        if (!returnSlide && isSliding)
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
        if (returnSlide && isSliding)
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

    void Delay()
    {
        if (delayCounter < delayTime && isDelayed) delayCounter += Time.time;
        else isDelayed = false;
    }

<<<<<<< HEAD
    void CheckCameraState()
    {
        if (cameraHolder.GetComponent<CameraHolder>().cameraState == 1) cameraState = 1;
        else cameraState = 0;
    }

    void SetUpgrade()
    {
        switch (upgrade)
        {
            case 0:                         //Tamagochi

                break;

            case 1:                         //Presilha

                break;

            case 2:                         //Relogio

                break;
            case 3:                         //cinto

                break;

        }
    }
    
=======
>>>>>>> parent of 54caebd (Adicionado a virada de camera)
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
}