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

            if (fasesCompletas == null || fasesCompletas.Length == 0)
                fasesCompletas = new bool[totalFases];

            LoadProgress();

            fasesCompletas[0] = true;
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
            PlayerPrefs.SetInt("Fase_" + indiceFase, 1);

            if (indiceFase + 1 < fasesCompletas.Length)
            {
                fasesCompletas[indiceFase + 1] = true;
                PlayerPrefs.SetInt("Fase_" + (indiceFase + 1), 1);
            }

            PlayerPrefs.Save();
        }
    }

    public bool FaseLiberada(int indiceFase)
    {
        return indiceFase >= 0 &&
               indiceFase < fasesCompletas.Length &&
               fasesCompletas[indiceFase];
    }

    public bool FaseCompletada(int indiceFase)
    {
        return PlayerPrefs.GetInt("Fase_" + indiceFase, 0) == 1;
    }

    void LoadProgress()
    {
        for (int i = 0; i < fasesCompletas.Length; i++)
        {
            fasesCompletas[i] = PlayerPrefs.GetInt("Fase_" + i, i == 0 ? 1 : 0) == 1;
        }
    }
}