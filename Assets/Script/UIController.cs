using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    static public UIController THIS;
    static public GameObject inGameUI;
    static public GameObject pauseUI;
    static public GameObject gameOverUI;
    static public GameObject winUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        THIS = this;
        inGameUI = OnlyOneTaggedObject("InGameUI");
        pauseUI = OnlyOneTaggedObject("PauseUI");
        gameOverUI = OnlyOneTaggedObject("GameOverUI");
        winUI = OnlyOneTaggedObject("WinUI");
        inGameUI.SetActive(true);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverUI.activeSelf && !winUI.activeSelf)
        {
            if (pauseUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    public static void GameOver()
    {
        Time.timeScale = 0;
        inGameUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(true);
        winUI.SetActive(false);
    }

    public static void Win()
    {
        Time.timeScale = 0;
        inGameUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        winUI.SetActive(true);
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        inGameUI.SetActive(true);
        pauseUI.SetActive(true);
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
    }

    public static void Resume()
    {
        Time.timeScale = 1;
        inGameUI.SetActive(true);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
    }

    public static void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    GameObject OnlyOneTaggedObject(string tag)
    {
        if (GameObject.FindGameObjectsWithTag(tag).Length > 1)
        {
            Debug.LogError("Multiple " + tag + " detected!");
            return null;
        }
        else
        {
            return GameObject.FindGameObjectWithTag(tag);
        }
    }
}
