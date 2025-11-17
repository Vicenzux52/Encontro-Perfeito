using UnityEngine;

public class FaseManager : MonoBehaviour
{
    public static FaseManager Instance;

    public bool[] fasesCompletas;
    public int totalFases = 3;

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
            PlayerPrefs.SetInt("FaseCompletada_" + indiceFase, 1);

            int proximaFase = indiceFase + 1;
            if (proximaFase < fasesCompletas.Length)
            {
                fasesCompletas[proximaFase] = true;
                PlayerPrefs.SetInt("FaseCompleta_" + proximaFase, 1);
            }

            SaveProgress();
        }
    }

    public bool FaseLiberada(int indiceFase)
    {
        return indiceFase >= 0 && indiceFase < fasesCompletas.Length && fasesCompletas[indiceFase];
    }

    public bool FaseCompletada(int indiceFase)
    {
        return PlayerPrefs.GetInt("FaseCompletada_" + indiceFase, 0) == 1;
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