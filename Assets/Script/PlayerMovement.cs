using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float routeDistance = 1f;
    int route = 0;
    public float speed = 1;
    bool isJumping;
    public float JumpForce = 10;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && route > 0)
        {
            route--;
        }
        if (Input.GetKeyDown(KeyCode.D) && route < 2)
        {
            route++;
        }
        
        SideDash();
        
        // Reset for testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            Jump();
        }
        if (transform.position.y <= 1.05)
        {
            isJumping = false;
        }
    }

    void SideDash()
    {
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(route * routeDistance, transform.position.y, transform.position.z), speed * Time.deltaTime);
        transform.Translate(route * routeDistance, transform.position.y, transform.position.z);
    }
    
    void Jump()
    {

    }
}
