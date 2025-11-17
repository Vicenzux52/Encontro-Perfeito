using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HeartsUI : MonoBehaviour
{
    public static HeartsUI Instance;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject winUI;
    public GameObject pauseUI;

    [Header("Photo Flash")]
    public Image flashImage;
    public float flashDuration = 0.3f;
    public AudioSource cameraSound;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHearts(int currentHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHearts)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    public void Win()
    {
        winUI.SetActive(true);
        if (cameraSound != null)
            cameraSound.Play();
        
        StartCoroutine(CameraFlashEffect());
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

    public void BackToFase()
    {
        if (DeathSaver.estereggConcluido)
        {
            SceneManager.LoadScene(DeathSaver.returnScene);
        }
        
        if (DeathSaver.estereggNaoConcluido)
        {
            SceneManager.LoadScene("Room");
        }
        else
        {
            SceneManager.LoadScene("EsterEgg");
        }
    }
    public void BackToRoom()
    {
        SceneManager.LoadScene("VideoPortal");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    public void BackGame()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }
}
