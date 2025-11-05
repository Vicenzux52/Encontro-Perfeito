using UnityEngine;
using UnityEngine.UI;

public class CalendarioManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public GameObject[] fases;

    private FaseManager progressManager;

    void Start()
    {
        progressManager = FaseManager.Instance;
        AtualizarBotoesFases();
        ConfigurarBotoesFases();
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
            Debug.LogError("RoomManager não encontrado na cena!");
        }
    }

    public void RefreshFases()
    {
        AtualizarBotoesFases();
    }
}