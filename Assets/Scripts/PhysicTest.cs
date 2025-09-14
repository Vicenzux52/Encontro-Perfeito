using NUnit.Framework;
using UnityEngine;

public class PhysicTest : MonoBehaviour
{
    Rigidbody rb;
    bool isSliding = false;
    bool returnSlide = false;
    public float slideAngle = 75f;
    public float slideSpeed = 5f;
    public float slideTime = 1f;
    float slideTimer = 0f;
    public float slideShakeForce = 3f;
    public float slideShakeSpeed = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isSliding)
        {
            isSliding = true;
        }
        if (isSliding)
        {
            Slide();
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * CalculateGravity(), ForceMode.Acceleration);
    }

    void Slide()
    {
        // fase 1: Descida 		| Se não na faixa do ângulo e não retornando
        if (!returnSlide /*&& não na faixa do angulo*/)
        {
            if (slideTimer == 0f) transform.RotateAround(transform.position, Vector3.right, -slideAngle * slideSpeed * Time.deltaTime);
            // fase 2: Parada com tremida	| Se no timer, não retornando
            if ((transform.eulerAngles.x <= 361 - slideAngle && transform.eulerAngles.x >= 35 - slideAngle) || slideTimer != 0f)
            {
                Debug.Log("Ponto baixo");
                slideTimer += Time.deltaTime;
                if (slideTimer > slideTime)
                {
                    slideTimer = 0f;
                    returnSlide = true;
                }
                transform.localRotation = Quaternion.Euler(new Vector3(-75 + Mathf.Sin(Time.time * slideShakeSpeed) * slideShakeForce, 0, 0));
            }
        }
        
        // fase 3: Subida		| Se timer > time Retornando
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

        /*if (!returnSlide && slideTimer == 0f) transform.RotateAround(transform.position, Vector3.right, -slideAngle * slideSpeed * Time.deltaTime);
        else if (returnSlide) transform.RotateAround(transform.position, Vector3.right, slideAngle * 0.75f * slideSpeed * Time.deltaTime);
        if (transform.eulerAngles.x <= 360 - slideAngle && transform.eulerAngles.x > 355 - slideAngle && !returnSlide)
        {
            Debug.Log("Start timer");
            slideTimer += Time.deltaTime;
            if (slideTimer > slideTime)
            {
                slideTimer = 0f;
                returnSlide = true;
            }
        }
        Debug.Log("Timer: " + slideTimer + " / " + slideTime);
        if (transform.eulerAngles.x > 0 && transform.eulerAngles.x < 5 && returnSlide)
        {

            returnSlide = false;
            isSliding = false;
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }*/
    }

    float CalculateGravity()
    {
        return 2 * 2 / (0.45f * 0.45f);
    }
}