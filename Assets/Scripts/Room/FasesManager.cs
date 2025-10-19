using UnityEngine;

public class FaseManager : MonoBehaviour
{
    public static FaseManager Instance;

    [Header("Progresso das Fases")]
    public bool[] fasesCompletas;
    public int totalFases = 4;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadProgress();

            if (fasesCompletas == null || fasesCompletas.Length == 0)
            {
                fasesCompletas = new bool[totalFases];

                if (fasesCompletas.Length > 0)
                {
                    fasesCompletas[0] = true;
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompletarFase(int indiceFase)
    {
        if (indiceFase >= 0 && indiceFase < fasesCompletas.Length)
        {
            fasesCompletas[indiceFase] = true;

            int proximaFase = indiceFase + 1;
            if (proximaFase < fasesCompletas.Length)
            {
                fasesCompletas[proximaFase] = true;
            }

            SaveProgress();
        }   
    }

    public bool FaseLiberada(int indiceFase)
    {
        return indiceFase >= 0 && indiceFase < fasesCompletas.Length && fasesCompletas[indiceFase];
    }

    void SaveProgress()
    {
        for (int i = 0; i < fasesCompletas.Length; i++)
        {
            PlayerPrefs.SetInt("FaseCompleta_" + i, fasesCompletas[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    void LoadProgress()
    {
        for (int i = 0; i < fasesCompletas.Length; i++)
        {
            fasesCompletas[i] = PlayerPrefs.GetInt("FaseCompleta_" + i, i == 0 ? 1 : 0) == 1;
        }
    }

}