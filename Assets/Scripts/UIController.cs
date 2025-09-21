using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    static public UIController THIS;
    //static bool[] collectibleCheck = new bool[3];
    public static bool[] collectibleCollected = new bool[3];
    //public Sprite[] collectibleImages = new Sprite[3];
    //public Sprite[] collectibleSilhouetteImages = new Sprite[3];
    //public GameObject[] collectibleObjects;
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
        //if (collectibleObjects.Length != 3) Debug.LogError("coletaveis na hud insuficientes");
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

        /*for (int i = 0; i <= 3; i++)
        {
            Debug.Log(i);
            if (collectibleCollected[i])
            {
                Debug.Log("detectei");
                collectibleObjects[i].GetComponent<Image>().sprite = collectibleImages[i];
            }
        }*/
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

    /*public static void RegisterCollectible(int index, Sprite collectibleImage, Sprite collectibleSilhouetteImage)   //Precisa de retrabalho
    {
        if (collectibleCheck[index] == false) collectibleCollected[index] = true;
        else Debug.LogError("Obstaculo com index repetido");
        collectibleImages[index] = collectibleImage;
        collectibleImages[index] = collectibleSilhouetteImage;
    }*/

    public static void Collect(int index)
    {
        collectibleCollected[index] = true;
        //Debug.Log("Check: " + collectibleCheck[0] + ", " + collectibleCheck[1] + ", " + collectibleCheck[2]);
        Debug.Log("Collected: " + collectibleCollected[0] + ", " + collectibleCollected[1] + ", " + collectibleCollected[2]);
    }
}
