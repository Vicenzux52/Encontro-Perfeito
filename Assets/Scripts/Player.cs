using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Quantidade de Rotas")]
    public float routeDistance = 1f;
    int routeQuantity = 1;
    [HideInInspector] public int route = 0;

    [Header("Movimento lateral")]
    public float frontSpeed = 0.1f;
    public float lateralSpeed = 1;

    [Header("Pulos")]
    public float peakHeight = 5f;
    public float peakTime = 2f;
    public float jumpCooldown = 0;
    public bool isJumping = false;
    
    [Header("FrontSlip")]
    bool onFrontSlip = false;
    public float frontSlipDistance = 1.5f;
    public float frontSlipInitial = 0;
    public float others = 1f;

    [Header("Delay")]
    public bool isDelayed = false;
    public float delayTime = 0;
    public float delaySpeed = 0.5f;

    //Outros
    Rigidbody rb;
    char returnOperation;
    float dSTemp;
    float dTTemp;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && route > -routeQuantity)
        {
            route--;
            returnOperation = '+';
        }
        if (Input.GetKeyDown(KeyCode.D) && route < routeQuantity)
        {
            route++;
            returnOperation = '-';
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
            isJumping = true;
        }

        SideDash();

        if (onFrontSlip)
        {
            FrontSlip();
        }
        if (isDelayed)
        {
            Delay();
        }

    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * CalculateGravity() * others, ForceMode.Acceleration);
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

    void SideDash()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(route * routeDistance,
        transform.position.y, transform.position.z), lateralSpeed * Time.deltaTime);
    }
    public void ReturnDash()
    {
        if (returnOperation == '+')
        {
            route++;
        }
        else if (returnOperation == '-')
        {
            route--;
        }
    }

    public void FrontSlip(float dT, float dS)
    {
        dTTemp = dT;
        dSTemp = dS;
        onFrontSlip = true;
        frontSlipInitial = transform.position.z;
        others = 1.5f;      
    }

    public void FrontSlip()
    {
        if (transform.position.z - frontSlipInitial > frontSlipDistance)
        {
            Debug.Log("Front slip ended and slowed down");
            onFrontSlip = false;
            others = 1f;
            delayTime = dTTemp;
            delaySpeed = dSTemp;
            isDelayed = true;
        }       
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, peakTime * CalculateGravity(), rb.linearVelocity.z);
    }

    public void Delay()
    {
        delayTime -= Time.deltaTime;
        if (delayTime <= 0)
        {
            isDelayed = false;
        }
    }

    float CalculateGravity()
    {
        return 2 * peakHeight / (peakTime * peakTime);
    }
}
