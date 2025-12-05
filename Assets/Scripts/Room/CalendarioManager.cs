using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarioManager : MonoBehaviour
{
    public GameObject[] fases;
    public Sprite[] buttonUnselect;
    public Sprite buttonSelect;

    private FaseManager progressManager;
    private int faseSelecionada = -1;
    private Button[] botoesFases;

    void Start()
    {
        Time.timeScale = 0f;
        progressManager = FaseManager.Instance;
        botoesFases = new Button[fases.Length];
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
                botoesFases[i] = botao;

                botao.GetComponent<Image>().sprite = buttonUnselect[i];

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
        if (faseSelecionada == indiceFase)
        {
            return;
        }

        if (faseSelecionada >= 0 && faseSelecionada < fases.Length && botoesFases[faseSelecionada] != null)
        {
            botoesFases[faseSelecionada].GetComponent<Image>().sprite = buttonUnselect[faseSelecionada];
        }

        faseSelecionada = indiceFase;

        if (botoesFases[indiceFase] != null)
        {
            botoesFases[indiceFase].GetComponent<Image>().sprite = buttonSelect;
        }

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
        AtualizarTextosFases();

        if (faseSelecionada >= 0 && faseSelecionada < botoesFases.Length && botoesFases[faseSelecionada] != null)
        {
            botoesFases[faseSelecionada].GetComponent<Image>().sprite = buttonSelect;
        }
    }
}