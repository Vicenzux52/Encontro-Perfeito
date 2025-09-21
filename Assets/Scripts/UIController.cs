using System.Threading;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    static public UIController THIS;
    static bool[] collectibleCheck = new bool[3];
    static bool[] collectibleCollected = new bool[3];
    static Image[] collectibleImages = new Image[3];
    static public GameObject inGameUI;
    static public GameObject pauseUI;
    static public GameObject gameOverUI;
    static public GameObject winUI;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
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

    public static void RegisterCollectible(int index, Image collectibleImage)
    {
        if (collectibleCheck[index] == false) collectibleCollected[index] = true;
        else Debug.LogError("Obstaculo com index repetido");
        collectibleImages[index] = collectibleImage;
    }

    public static void collect(int index)
    {
        collectibleCollected[index] = true;
    }

}
