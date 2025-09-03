using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float delay = 1f;
    public float delaySpeed = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision detected");
            if (Mathf.Abs(other.transform.position.x - transform.position.x) <= 0.5f)
            {
                UIController.GameOver();
            }
            else
            {
                other.GetComponent<Player>().delayTime = delay;
                other.GetComponent<Player>().delaySpeed = delaySpeed;
                other.GetComponent<Player>().isDelayed = true;
                other.GetComponent<Player>().ReturnDash();
            }
        }
    }
}
