using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject audioPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public AudioSource audioSource;

    public void Play()
    {
        audioSource.Play();
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        audioSource.Play();
        Debug.Log("Fechou");
        Application.Quit();
    }

    public void Audio()
    {
        audioSource.Play();
        audioPanel.SetActive(true);
    }

    public void Controls()
    {
        audioSource.Play();
        controlsPanel.SetActive(true);
    }

    public void Credits()
    {
        audioSource.Play();
        creditsPanel.SetActive(true);
    }

    public void BackButton()
    {
        audioSource.Play();
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    void Start()
    {
        PlayerPrefs.DeleteKey("UpgradeID");
        PlayerPrefs.DeleteKey("paqueraSelect");
        PlayerPrefs.DeleteKey("petSelect");
        PlayerPrefs.Save();

        Debug.Log("Upgrades e escolhas resetados!");
    }
}

