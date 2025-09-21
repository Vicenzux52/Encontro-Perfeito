using UnityEngine;
using TMPro;

public class GameStartTimer : MonoBehaviour
{
    public float startTime = 3f;
    public TextMeshProUGUI timerText;
    private float endTime;
    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f;

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
            Time.timeScale = 1f;
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
