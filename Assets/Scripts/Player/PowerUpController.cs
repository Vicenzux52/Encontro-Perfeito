using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public Timer timer;
    public float extraTime;
    AudioSource powerupSound;

    public void Start()
    {
        powerupSound = transform.Find("PowerUpAudio").GetComponent<AudioSource>();
        if (powerupSound == null)
        {
            powerupSound = gameObject.AddComponent<AudioSource>();
        }
        //timer = GetComponent<Timer>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClockPowerUp"))
        {
            timer.timeLeft += extraTime;
            powerupSound.Play();
            Destroy(other.gameObject);
        }
    }
}
