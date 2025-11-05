using System;
using Unity.Mathematics;
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
    public float backDash = 20f;
    public float limitSpeed = 100;
    bool onMaxSpeed = false;

    [Header("Pulos")]
    public float JumpHeight = 5f;
    public float jumpDuration = 2f;
    float gravity = 10;
    float initialYJump = 0;
    float timeX = 0;
    bool isJumping = false;
    float down = 1;

    [Header("Delay")]
    public float delayForce = 30f;
    bool isDelayed = false;
    public float lateralSpeedDelay = 0.5f;
    public float delayTime = 1;
    float delayCounter = 0; //deve tirar

    [Header("Slide")]
    [SerializeField] float slideAngle = 75f;
    [SerializeField] float slideRotationSpeed = 75f;
    [SerializeField] float slideTime = 1f;
    Quaternion targetRotation;
    bool isSliding = false;
    float slideTimer = 0f; //deve tirar
    float up = 1;

    [Header("Hit")]
    public Material hitMaterial;
    private Material originalMaterial;
    private Renderer rend;
    public float hitDuration = 3f;
    private bool isHit = false;

    [Header("Maze")]
    public float positionX = 0;
    public float positionY = 0;
    public float limitX = 15;
    public float limitY = 15;
    public float centerY;
        
    int upgrade;
    //Outros
    int cameraState = 0;
    Rigidbody rb;
    GameObject cameraHolder;
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

        upgrade = PlayerPrefs.GetInt("UpgradeID", -1);
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
        
        cameraHolder = Camera.main.transform.parent.gameObject;
    }

    void Update()
    {
        GetInputs();
        CheckCameraState();
        /*if (cameraState > 1)
        {*/
            Slide();
            FrontalMovement();
            SideDash();
            Delay();
            Jump();
        /*}
        else
        {
            MazeMovement();
        }*/
    }

    void FixedUpdate()
    {
        if (!isJumping) rb.AddForce(-orientation.up * gravity, ForceMode.Acceleration);
    }
    
    void GetInputs()
    {
        bool leftInputs = (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && route > -routeQuantity;
        bool rightInputs = (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && route < routeQuantity;
        bool jumpInputs = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && !isSliding && !isJumping; //Decidir se o pulo cancela o slide ou só pula mesmo
        bool upInputs = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && !isSliding && !isJumping;
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

        if (jumpInputs)
        {
            initialYJump = transform.position.y;
            isJumping = true;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (upInputs)
        {
            up += 0.05f;
        }

        if (slideInputs)
        {
            isSliding = true;
        }

        if (downInputs)
        {
            down += 0.05f;
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
        if (cameraState == 0)
        {
            route = Mathf.Clamp(route, -routeQuantity, routeQuantity);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(route * routeDistance,
            transform.position.y, transform.position.z), lateralSpeed * Time.deltaTime);
        }
        else
        {
            if (route == -1)
            {
                onMaxSpeed = false;
                rb.linearVelocity = -orientation.forward * backDash;
            }
            route = 0;
        }
    }

    void Jump()
    {
        if (isJumping)
        {
            transform.position = new Vector3(transform.position.x, initialYJump + JumpHeight * Mathf.Pow(Mathf.Sin(timeX / jumpDuration), 1f / 2f),
            transform.position.z);
            timeX += down * Time.deltaTime;
            if (Mathf.Sin(timeX / jumpDuration) < 0)
            {
                down = 1;
                isJumping = false;
                timeX = 0;
            }
        }
    }

    void Slide()
    {
        if (isSliding)
        {
            targetRotation = Quaternion.Euler(-slideAngle, 0, 0);
            slideTimer += up * Time.deltaTime;
            if (slideTimer > slideTime)
            {
                slideTimer = 0f;
                up = 1f;
                isSliding = false;
            }
        }
        if (!isSliding)
        {
            targetRotation = Quaternion.Euler(0, 0, 0);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, slideRotationSpeed * Time.deltaTime);
    }

    void Delay()
    {
        if (delayCounter < delayTime && isDelayed) delayCounter += Time.time;
        else isDelayed = false;
    }

    void CheckCameraState()
    {
        cameraState = cameraHolder.GetComponent<CameraHolder>().cameraState;
        /*if (cameraState > 0 && cameraHolder.GetComponent<CameraHolder>().onTransition) 
        {
            route = 0;
            transform.position = transform.position - (Vector3.right * transform.position.x);
            positionX = cameraHolder.GetComponent<CameraHolder>().InitialPositionX;
            positionY = cameraHolder.GetComponent<CameraHolder>().InitialPositionY;
            limitX = cameraHolder.GetComponent<CameraHolder>().limitX;
            limitY = cameraHolder.GetComponent<CameraHolder>().limitY;
            centerY = cameraHolder.GetComponent<CameraHolder>().centerY;
        }*/
    }

    /*void MazeMovement()
    {
        //fazer uma movimentação que funcione, talvez que nem helltaker
        positionX = Mathf.Clamp(positionX, -limitX, limitX);
        positionY = Mathf.Clamp(positionY, -limitY, limitY);
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(route * routeDistance,
        transform.position.y, transform.position.z), lateralSpeed * Time.deltaTime);
    }*/
    
    void SetUpgrade()
    {
        switch (upgrade)
        {
            case 0:                         //Tamagochi
                JumpHeight *= 2;
                limitSpeed /= 2;
                break;

            case 1:                         //Relogio
                lateralSpeed -= 5;
                break;

            case 2:                         //Presilha
                //tem que ver ainda
                break;
            case 3:                         //cinto
                limitSpeed += 10;
                break;

        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        /*if (cameraState < 2)
        {*/
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
    
                if (isJumping) down *= 2;
    
                if (!isHit)
                {
                    StartCoroutine(FlashMaterial());
                }
            }
        /*}
        else
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                //Descobrir como fazer o retorno sem bugar com a velocidade
            }
        }*/
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
