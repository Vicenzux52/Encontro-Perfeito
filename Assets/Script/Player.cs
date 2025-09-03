using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Quantidade de Rotas")]
    public float routeDistance = 1f;
    public int routeQuantity = 2;
    int route = 0;

    [Header("Movimento lateral")]
    public float frontSpeed = 0.1f;
    public float lateralSpeed = 1;

    [Header("Pulos")]
    public float peakHeight = 5f;
    public float peakTime = 2f;
    public float jumpCooldown = 0;
    bool isJumping = false;
    

    //Outros
    Rigidbody rb;
    char returnOperantion;
    public bool isDelayed = false;
    public float delayTime = 0;
    public float delaySpeed = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && route > 0)
        {
            route--;
            returnOperantion = '+';
        }
        if (Input.GetKeyDown(KeyCode.D) && route < routeQuantity)
        {
            route++;
            returnOperantion = '-';
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
            isJumping = true;
        }

        SideDash();

        if (isDelayed)
        {
            Delay();
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * CalculateGravity(), ForceMode.Acceleration);
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
        if (returnOperantion == '+')
        {
            route++;
        }
        else if (returnOperantion == '-')
        {
            route--;
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
