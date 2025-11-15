using UnityEngine;

public class TutorialRoom : MonoBehaviour
{
    [Header("Painéis do Tutorial")]
    public GameObject[] panels;

    [Header("Botão Skip (opcional)")]
    public GameObject skipButton;

    private int currentPanelIndex = 0;
    [HideInInspector] public bool finishTutorial = false;

    void Start()
    {
        foreach (var p in panels)
            if (p != null)
                p.SetActive(false);

        if (PlayerPrefs.GetInt("TutorialVisto", 0) == 1)
        {
            finishTutorial = true;
            if (skipButton != null)
                skipButton.SetActive(false);
            return;
        }

        ShowCurrentPanel();

        if (skipButton != null)
            skipButton.SetActive(true);
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
            FinalizarTutorial();
        }
    }

    private void FinalizarTutorial()
    {
        finishTutorial = true;

        foreach (var p in panels)
            if (p != null)
                p.SetActive(false);

        if (skipButton != null)
            skipButton.SetActive(false);

        PlayerPrefs.SetInt("TutorialVisto", 1);
        PlayerPrefs.Save();

        Debug.Log("[TutorialRoom] Tutorial finalizado e salvo!");

        RoomManager roomManager = FindFirstObjectByType<RoomManager>();
        if (roomManager != null)
            Debug.Log("[TutorialRoom] RoomManager notificado do fim do tutorial.");
    }

    private void ShowCurrentPanel()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null)
                panels[i].SetActive(i == currentPanelIndex);
        }
    }
}