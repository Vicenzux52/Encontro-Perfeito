using UnityEngine;
using TMPro;
using System.Collections;

public class Fantasma : MonoBehaviour
{
    [Header("Rotas")]
    [SerializeField] float routeDistance = 2f;
    [SerializeField] int routeQuantity = 1;
    [HideInInspector] public int route = 0;

    [Header("Movimento")]
    public float frontSpeed = 35f;
    public float lateralSpeed = 15;
    public float backDash = 80f;
    public float limitSpeed = 40;
    public float acceleration = 100;
    public float knockbackMultiplier = 2.5f;
    public bool canMove = false;

    [Header("Pulos")]
    public float JumpHeight = 2f;
    public float jumpDuration = 0.3f;
    float gravity = 10;
    float initialYJump = 0;
    float timeX = 0;
    bool isJumping = false;
    float down = 1;

    [Header("Delay")]
    public float delayForce = 50f;
    bool isDelayed = false;
    public float lateralSpeedDelay = 0.5f;
    public float delayTime = 1;
    float delayCounter = 0;

    [Header("Hit")]
    public int maxCollisions = 3; 
    public int maxHearts = 3;
    public int currentHearts;
    private bool recentlyHit = false;
    public float hitCooldown = 0.2f;

    [Header ("Game Start Time")]
    public float startTime = 3f;
    public TextMeshProUGUI timerText;
    private float endTime;
    private bool gameStarted = false;
    public GameObject player;
    Vector3 initialPosition;
        
    //Outros
    int cameraState = 0;
    Rigidbody rb;
    GameObject cameraHolder;
    Transform orientation;

    [Header("Audio")]
    public AudioSource audioSource;

    public UIController uIController;

    void Start()
    {
        currentHearts = maxHearts;
        HeartsUI.Instance.UpdateHearts(currentHearts);

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).name == "Orientation") orientation = transform.GetChild(i);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        cameraHolder = Camera.main.transform.parent.gameObject;

        initialPosition = player.transform.position;

        endTime = Time.realtimeSinceStartup + startTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (canMove)
        {
            GetInputs();
            CheckCameraState();
            FrontalMovement();
            SideDash();
            Jump();
        }
    }
    
    void FixedUpdate()
    {
        if (!isJumping) rb.AddForce(-orientation.up * gravity, ForceMode.Acceleration);
    }

    void GetInputs()
    {
        bool leftInputs = (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && route > -routeQuantity;
        bool rightInputs = (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && route < routeQuantity;
        bool jumpInputs = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping;
        bool upInputs = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && !isJumping;

        if (leftInputs)
        {
            route--;
            DashSound();
        }

        if (rightInputs)
        {
            route++;
            DashSound();
        }

        if (jumpInputs)
        {
            initialYJump = transform.position.y;
            isJumping = true;
            DashSound();
        }
    }

    void DashSound()
    {

        audioSource.PlayOneShot(audioSource.clip);
    }

    void FrontalMovement()
    {
        Debug.Log("Andando");
        frontSpeed += acceleration * Time.deltaTime;
        if (frontSpeed < 0) frontSpeed += acceleration * Time.deltaTime;
        if (frontSpeed > limitSpeed) frontSpeed = limitSpeed;
        if (isDelayed) frontSpeed =  limitSpeed * lateralSpeedDelay;
        transform.position += Vector3.forward * frontSpeed * Time.deltaTime;
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
            if (route == -1 && !cameraHolder.GetComponent<CameraHolder>().onTransition) frontSpeed = -backDash;
            route = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, transform.position.z), lateralSpeed * Time.deltaTime);
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

    void CheckCameraState()
    {
        cameraState = cameraHolder.GetComponent<CameraHolder>().cameraState;
    }
    
    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Obstacle"))
    {
        if (!recentlyHit)
        {
            TakeDamage(1);
            StartCoroutine(HitCooldown());
        }
    }
}

    IEnumerator HitCooldown()
    {
        recentlyHit = true;
        yield return new WaitForSeconds(hitCooldown);
        recentlyHit = false;
    }

    public void TakeDamage(int amount = 1)
    {
        Debug.Log("DANO: " + currentHearts);

        currentHearts -= amount;

        if (currentHearts < 0)
            currentHearts = 0;

        HeartsUI.Instance.UpdateHearts(currentHearts);

        if (currentHearts <= 0)
        {
            UIController.GameOver();
        }
    }

    void LateUpdate()
    {
        if (gameStarted) return;

        float remaining = endTime - Time.realtimeSinceStartup;

        if (remaining > 0f)
        {
            timerText.text = Mathf.CeilToInt(remaining).ToString();
        }
        else
        {
            timerText.text = "Já!";
            gameStarted = true;
            Invoke(nameof(HideTimer), 0.5f);
            player.GetComponent<Fantasma>().canMove = true;
        }
    }

    void UpdateTimerText()
    {
        float remaining = endTime - Time.realtimeSinceStartup;
        timerText.text = Mathf.CeilToInt(remaining).ToString();
    }

    void HideTimer()
    {
        timerText.gameObject.SetActive(false);
    }
}
