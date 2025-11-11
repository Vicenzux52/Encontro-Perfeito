using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float initialTime = 60.0f;
    public float timeLeft;
    public Text timerText;
    int upgrade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = initialTime;
        timerText = gameObject.GetComponent<Text>();
        
        upgrade = PlayerPrefs.GetInt("UpgradeID", -1);
        SetUpgrade();
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
        seconds = (int)seconds;
        if (seconds <= 0)
        {
            return "00:00";
        }
        else if (seconds < 10)
        {
            return "00:0" + seconds;
        }
        else if (seconds < 60)
        {
            return "00:" + seconds;
        }
        else if (seconds / 60 < 10)
        {
            return "0" + seconds / 60 + ":" + (seconds % 60);
        }
        else if (seconds / 60 < 60)
        {
            return seconds / 60 + ":" + (seconds % 60);
        }
        else
        {
            return seconds / 3600 + ":" + seconds % 3600 / 60 + ":" + (seconds % 60);
        }
    }

    void SetUpgrade()
    {
        switch (upgrade)
        {
            case 1:                         //Relogio
                timeLeft += 30;
                break;

            case 2:                         //Presilha
                //tem que ver ainda
                break;
            case 3:                         //cinto
                timeLeft -= 30;
                break;

        }
    }
}
