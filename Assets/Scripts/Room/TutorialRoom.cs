using UnityEngine;

public class TutorialRoom : MonoBehaviour
{
    public GameObject[] panels;
    private int currentPanelIndex = 0;

    [HideInInspector] public bool finishTutorial = false;

    void Start()
    {
        if (PlayerPrefs.GetInt("TutorialVisto", 0) == 1)
        {
            finishTutorial = true;
            foreach (var p in panels)
                p.SetActive(false);
            return;
        }

        ShowCurrentPanel();
    }

    public void SkipPanel()
    {
        if (currentPanelIndex < panels.Length)
            panels[currentPanelIndex].SetActive(false);

        currentPanelIndex++;

        if (currentPanelIndex < panels.Length)
        {
            panels[currentPanelIndex].SetActive(true);
        }
        else
        {
            finishTutorial = true;
            Debug.Log("Tutorial finalizado!");

            foreach (var p in panels)
                p.SetActive(false);

            PlayerPrefs.SetInt("TutorialVisto", 1);
            PlayerPrefs.Save();

            GameObject skipButton = GameObject.Find("SkipButton");
            if (skipButton != null)
                skipButton.SetActive(false);
        }

        if (finishTutorial)
        {
            RoomManager roomManager = FindFirstObjectByType<RoomManager>();
            if (roomManager != null)
            {
                Debug.Log("Tutorial finalizado");
            }
        }
    }

    private void ShowCurrentPanel()
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(i == currentPanelIndex);
    }
}
