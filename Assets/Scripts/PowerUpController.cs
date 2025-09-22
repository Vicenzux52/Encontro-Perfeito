using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public Timer timer;
    public float extraTime;

    public void Start()
    {
        //timer = GetComponent<Timer>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClockPowerUp"))
        {
            timer.timeLeft += extraTime;
            Destroy(other.gameObject);
        }
    }
}
