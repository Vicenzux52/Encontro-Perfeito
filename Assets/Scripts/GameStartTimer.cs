using UnityEngine;
using TMPro;

public class GameStartTimer : MonoBehaviour
{
    public float startTime = 3f;
    public TextMeshProUGUI timerText;
    private float endTime;
    private bool gameStarted = false;
    public GameObject player; 
    private Rigidbody playerRb;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        playerRb.isKinematic = true;

        endTime = Time.realtimeSinceStartup + startTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (gameStarted) return;

        float remaining = endTime - Time.realtimeSinceStartup;

        if (remaining > 0f)
        {
            timerText.text = Mathf.CeilToInt(remaining).ToString();
        }
        else
        {
            timerText.text = "Já!";
            gameStarted = true;
            playerRb.isKinematic = false;
             playerRb.linearVelocity = Vector3.forward * player.GetComponent<Player>().limitSpeed;
            Invoke(nameof(HideTimer), 0.5f);
        }
    }

    void UpdateTimerText()
    {
        float remaining = endTime - Time.realtimeSinceStartup;
        timerText.text = Mathf.CeilToInt(remaining).ToString();
    }

    void HideTimer()
    {
        timerText.gameObject.SetActive(false);
    }
}
