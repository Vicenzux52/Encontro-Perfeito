using System;
using UnityEngine;

public class PhysicTest : MonoBehaviour
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
        }
        
        //velocity.z = limitSpeed;
    }

    void Update()
    {
        Debug.Log(velocity);
        velocity = Vector3.zero;
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && route > -routeQuantity)
        {
            route--;
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && route < routeQuantity)
        {
            route++;
        }
        if (cC.isGrounded) isJumping = true;

        else isJumping = false;
    }

    void FixedUpdate()
    {
        velocity.y -= CalculateGravity() * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);
    }

    /* void FrontalMovement()
    {
        velocity.z = Mathf.Min(velocity.z + frontSpeed/10 * Time.deltaTime, limitSpeed/10);
        if (isDelayed)
            velocity.z = limitSpeed * lateralSpeedDelay;
    } */

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

    float CalculateGravity()
    {
        return 2 * peakHeight / (peakTime * peakTime);
    }
}

