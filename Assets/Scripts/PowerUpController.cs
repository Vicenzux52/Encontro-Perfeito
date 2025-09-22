using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public Timer timer;
    public float extraTime;
    AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        //timer = GetComponent<Timer>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClockPowerUp"))
        {
            timer.timeLeft += extraTime;
            audioSource.Play();
            Destroy(other.gameObject);
        }
    }
}
