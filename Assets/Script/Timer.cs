using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float initialTime = 60.0f;
    public float timeLeft;
    public Text timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = initialTime;
        timerText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = TimeFormat(timeLeft);
        }
        else
        {
            UIController.GameOver();
        }

    }

    string TimeFormat(float seconds)
    {
        if (seconds < 0)
        {
            return "00:00";
        }
        else if (seconds < 10)
        {
            return "00:0" + (int)seconds;
        }
        else if (seconds < 60)
        {
            return "00:" + (int)seconds;
        }
        else if ((int)seconds / 60 < 10)
        {
            return "0" + (int)seconds / 60 + ":" + (int)(seconds % 60);
        }
        else if ((int)seconds / 60 < 60)
        {
            return (int)seconds / 60 + ":" + (int)(seconds % 60);
        }
        else
        {
            return (int)seconds / 3600 + ":" + (int)(seconds % 3600) / 60 + ":" + (int)(seconds % 60);
        }
    }
}
