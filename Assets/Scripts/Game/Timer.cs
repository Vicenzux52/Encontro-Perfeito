using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float initialTime = 60.0f;
    public float timeLeft;
    public Text timerText;
    int seconds;
    int minutes;
    int upgrade;
    Player player;
    public AudioSource gameOverAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        timeLeft = initialTime;
        timerText = gameObject.GetComponent<Text>();
        
        upgrade = PlayerPrefs.GetInt("UpgradeID", -1);
        SetUpgrade();
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            if (player.canMove)timeLeft -= Time.deltaTime;
            timerText.text = TimeFormat(timeLeft);
        }
        else
        {
            gameOverAudio.Play();
            PhotoAlbumManager.isGameOverTime = true;
            UIController.GameOver();
        }
    }

    string TimeFormat(float time)
    {
        seconds = (int)time % 60;
        minutes = (int)time / 60;
        if (minutes >= 10)
        {
            if (seconds >= 10) return minutes + ":" + seconds;
            else return minutes + ":0" + seconds;
        }
        else if (minutes >= 1)
        {
            if (seconds >= 10) return "0" + minutes + ":" + seconds;
            else return "0" + minutes + ":0" + seconds;
        }
        else
        {
            if (seconds >= 10) return "00:" + seconds;
            else return "00:0" + seconds;
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
