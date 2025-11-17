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
    static public GameObject gameOverDeathUI;
    static public GameObject gameOverTimerUI;

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
    public AudioSource gameOverAudio;

    [Header("Fase Configuration")]
    public int indiceFaseAtual = 0;
    public bool[] faseCompletada = new bool[3];

    void Start()
    {
        THIS = this;
        inGameUI = OnlyOneTaggedObject("InGameUI");
        pauseUI = OnlyOneTaggedObject("PauseUI");
        gameOverDeathUI = OnlyOneTaggedObject("GameOverDeathUI");
        gameOverTimerUI = OnlyOneTaggedObject("GameOverTimerUI");
        winUI = OnlyOneTaggedObject("WinUI");
        inGameUI.SetActive(true);
        gameOverTimerUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverDeathUI.SetActive(false);
        winUI.SetActive(false);
        //if (collectibleObjects.Length != 3) Debug.LogError("coletaveis na hud insuficientes");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverDeathUI.activeSelf && !winUI.activeSelf)
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

    public void GameOverDeath()
    {
        Time.timeScale = 0;
        if (THIS != null && THIS.gameOverAudio != null)
            THIS.gameOverAudio.Play();
        inGameUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverDeathUI.SetActive(true);
        gameOverTimerUI.SetActive(false);
        winUI.SetActive(false);
    }

    public void GameOverTimer()
    {
        Time.timeScale = 0;
        if (THIS != null && THIS.gameOverAudio != null)
            THIS.gameOverAudio.Play();
        inGameUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverTimerUI.SetActive(true);
        gameOverDeathUI.SetActive(false);
        winUI.SetActive(false);
    }

    public void Win()
    {
        string paqueraEscolhida = PlayerPrefs.GetString("paqueraSelect", "Feminino");

        Time.timeScale = 0;
        inGameUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverTimerUI.SetActive(false);
        gameOverDeathUI.SetActive(false);
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


    public void Pause()
    {
        Time.timeScale = 0;
        inGameUI.SetActive(true);
        pauseUI.SetActive(true);
        gameOverDeathUI.SetActive(false);
        gameOverTimerUI.SetActive(false);
        winUI.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        inGameUI.SetActive(true);
        pauseUI.SetActive(false);
        gameOverDeathUI.SetActive(false);
        gameOverTimerUI.SetActive(false);
        winUI.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void BackToRoom()
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

    public void ResumeButton() { Resume(); }
    public void PauseButton() { Pause(); }
    public void RestartButton() { Restart(); }
    public void BackToRoomButton() { BackToRoom(); }
    public void GameOverDeathButton() { GameOverDeath(); }
    public void GameOverTimerButton() { GameOverTimer(); }
    public void WinButton() { Win(); }
}