using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject audioPanel;
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

    public void BackButton()
    {
        audioPanel.SetActive(false);
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

