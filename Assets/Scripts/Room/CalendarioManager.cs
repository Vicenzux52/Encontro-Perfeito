using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarioManager : MonoBehaviour
{
    public GameObject[] fases;

    private FaseManager progressManager;

    void Start()
    {
        Time.timeScale = 0f;
        progressManager = FaseManager.Instance;
        AtualizarBotoesFases();
        ConfigurarBotoesFases();
        AtualizarTextosFases();
    }

    void AtualizarBotoesFases()
    {
        for (int i = 0; i < fases.Length; i++)
        {
            if (fases[i] != null)
            {
                bool faseLiberada = progressManager.FaseLiberada(i);
                fases[i].SetActive(faseLiberada);
            }
        }
    }

    void AtualizarTextosFases()
    {
        for (int i = 0; i < fases.Length; i++)
        {
            if (fases[i] != null)
            {
                TextMeshProUGUI textF = fases[i].GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }

    void ConfigurarBotoesFases()
    {
        for (int i = 0; i < fases.Length; i++)
        {
            if (fases[i] != null)
            {
                Button botao = fases[i].GetComponent<Button>();
                if (botao != null)
                {
                    int indice = i;
                    botao.onClick.AddListener(() => SelecionarFase(indice));
                }
            }
        }
    }

    public void SelecionarFase(int indiceFase)
    {
        RoomManager roomManager = FindFirstObjectByType<RoomManager>();
        if (roomManager != null)
        {
            roomManager.SelecionarFase(indiceFase);
        }
        else
        {
            Debug.LogError("RoomManager n�o encontrado na cena!");
        }
    }

    public void RefreshFases()
    {
        AtualizarBotoesFases();
        AtualizarTextosFases();
    }
}