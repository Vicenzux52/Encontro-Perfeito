using System.Collections;
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

    [Header("Photo Win")]
    public int photoIndex;
    static public GameObject winUI;
    public Image MaskImage;
    public Sprite[] maskSprites;
    public Image PhotoBaseF;
    public Image PhotoBaseM;

    [Header("Photo Flash")]
    public Image flashImage;
    public float flashDuration = 0.3f;
    public AudioSource cameraSound;

    [Header("Fase Configuration")]
    public int indiceFaseAtual = 0;
    public bool[] faseCompletada = new bool[3];

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

    public void Win()
    {
        string paqueraEscolhida = PlayerPrefs.GetString("paqueraSelect", "Feminino");

        Time.timeScale = 0;
        inGameUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        winUI.SetActive(true);

        if (cameraSound != null)
            cameraSound.Play();
        
        StartCoroutine(CameraFlashEffect());

        if (paqueraEscolhida == "Feminino")
        {
            PhotoBaseF.gameObject.SetActive(true);
            PhotoBaseM.gameObject.SetActive(false);
        }
        else
        {
            PhotoBaseF.gameObject.SetActive(false);
            PhotoBaseM.gameObject.SetActive(true);
        }

        int collectedParts = CollectibleProgress.photoPartsCollected[photoIndex];

        if (collectedParts >= maskSprites.Length)
        {
            MaskImage.enabled = false;
        }
        else
        {
            MaskImage.enabled = true;
            int maskIndex = Mathf.Clamp(collectedParts, 0, maskSprites.Length - 1);
            MaskImage.sprite = maskSprites[maskIndex];
        }

        CompletarFaseAtual();
    }

    IEnumerator CameraFlashEffect()
    {
        flashImage.color = new Color(1, 1, 1, 1);
        flashImage.enabled = true;

        for (float t = 0; t < flashDuration; t += Time.unscaledDeltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / flashDuration);
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        flashImage.color = new Color(1, 1, 1, 0);
        flashImage.enabled = false;
    }
    
    private void CompletarFaseAtual()
    {
        if (indiceFaseAtual >= 0 && indiceFaseAtual < 3)
        {
            if (FaseManager.Instance != null)
            {
                FaseManager.Instance.CompletarFase(indiceFaseAtual);
            }
            else
            {
                Debug.LogWarning("FaseManager.Instance é nulo — progresso não foi salvo.");
            }

            PlayerPrefs.SetInt($"Fase{indiceFaseAtual}", 1);
            PlayerPrefs.SetInt("AtualizarMensagens", 1);
            PlayerPrefs.Save();

            Debug.Log($"✅ Fase {indiceFaseAtual + 1} completada!");
        }
        else
        {
            Debug.LogWarning("Índice de fase inválido!");
        }

        var roomManager = FindFirstObjectByType<RoomManager>();
        if (roomManager != null)
        {
            roomManager.MostrarMensagemPorFase();
            Debug.Log("Mensagens de fase atualizadas pelo RoomManager.");
        }
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

    public static void BackToRoom()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Room");
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
    }
}