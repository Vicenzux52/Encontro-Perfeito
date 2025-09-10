using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float delayTime = 1f;
    public float delaySpeed = 0.5f;
    public bool canStayOnTop = false;
    Collider collide;
    float widthRadius;
    float heightRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collide = GetComponent<BoxCollider>();
        if (canStayOnTop)
        {
            collide.isTrigger = false;
        }
        else
        {
            collide.isTrigger = true;
        }
        widthRadius = collide.bounds.size.x / 2;
        heightRadius = collide.bounds.size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float playerHeightRadius = other.GetComponent<Collider>().bounds.size.y / 2;
            Debug.Log("Collision detected");
            if (other.transform.position.y - playerHeightRadius > transform.position.y)
            {
                // Pushes the player forward and then slows down
                other.GetComponent<Player>().FrontSlip(delayTime, delaySpeed);
            }
            else if (Mathf.Abs(other.transform.position.x - transform.position.x) >= widthRadius / 2)
            {
                // Return the player dash
                other.GetComponent<Player>().delayTime = delayTime;
                other.GetComponent<Player>().delaySpeed = delaySpeed;
                other.GetComponent<Player>().isDelayed = true;
                // if de quando não se deve retornar o dash
                other.GetComponent<Player>().ReturnDash();
            }
            else
            {
                UIController.GameOver();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float playerHeightRadius = collision.gameObject.GetComponent<Collider>().bounds.size.y / 2;
            if (collision.transform.position.y - playerHeightRadius > transform.position.y + heightRadius*0.15f)
            {
                // Allow the player to stay on top of the obstacle
                collision.gameObject.GetComponent<Player>().isJumping = false;
                collision.gameObject.GetComponent<Player>().jumpCooldown = 0;
            }
            else if (Mathf.Abs(collision.transform.position.x - transform.position.x) >= widthRadius / 2)
            {
                // Return the player dash
                collision.gameObject.GetComponent<Player>().delayTime = delayTime;
                collision.gameObject.GetComponent<Player>().delaySpeed = delaySpeed;
                collision.gameObject.GetComponent<Player>().isDelayed = true;
                // if de quando não se deve retornar o dash
                collision.gameObject.GetComponent<Player>().ReturnDash();
            }
            else
            {
                UIController.GameOver();
            }
        }
    }

}
