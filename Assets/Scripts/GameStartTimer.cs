using UnityEngine;
using TMPro;

public class GameStartTimer : MonoBehaviour
{
    public float startTime = 3f;
    public TextMeshProUGUI timerText;
    private float endTime;
    private bool gameStarted = false;
    public GameObject player;
    Vector3 initialPosition;

    void Start()
    {
        initialPosition = player.transform.position;

        endTime = Time.realtimeSinceStartup + startTime;
        UpdateTimerText();
    }

    void LateUpdate()
    {
        if (gameStarted) return;

        float remaining = endTime - Time.realtimeSinceStartup;

        if (remaining > 0f)
        {
            player.transform.position = initialPosition;
            timerText.text = Mathf.CeilToInt(remaining).ToString();
        }
        else
        {
            timerText.text = "Já!";
            gameStarted = true;
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
