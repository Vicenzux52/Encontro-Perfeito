using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject audioPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        Debug.Log("Fechou");
        Application.Quit();
    }

    public void Audio()
    {
        audioPanel.SetActive(true);
    }

    public void Controls()
    {
        controlsPanel.SetActive(true);
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
    }

    public void BackButton()
    {
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

