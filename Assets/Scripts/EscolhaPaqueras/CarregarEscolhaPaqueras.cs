using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarEscolhaPaqueras : MonoBehaviour
{
    [Header("Paqueras")]
    public GameObject PaqueraF;
    public GameObject PaqueraM;
    [HideInInspector] public string paquera;

    void Start()
    {
        paquera = PlayerPrefs.GetString("paqueraSelect", "Feminino");

        PaqueraF.SetActive(paquera == "Feminino");
        PaqueraM.SetActive(paquera == "Masculino");

        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log($"Paquera: {paquera} // Cena: {currentScene}");
    }
}
