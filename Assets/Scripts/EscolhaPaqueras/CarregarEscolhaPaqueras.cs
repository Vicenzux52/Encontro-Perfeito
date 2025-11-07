using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarEscolhaPaqueras : MonoBehaviour
{
    [Header("Paqueras and Pets")]
    public GameObject PaqueraF;
    public GameObject PaqueraM;
    public GameObject Rabbit;
    public GameObject Cat;
    public GameObject Tortoise;
    [HideInInspector] public string paquera;
    [HideInInspector] public string pet;

    void Start()
    {
        paquera = PlayerPrefs.GetString("paqueraSelect", "Feminino");
        pet = PlayerPrefs.GetString("petSelect", "Gato");

        PaqueraF.SetActive(paquera == "Feminino");
        PaqueraM.SetActive(paquera == "Masculino");

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Fase3")
        {
            Rabbit.SetActive(pet == "Coelho");
            Cat.SetActive(pet == "Gato");
            Tortoise.SetActive(pet == "Tartaruga");
        }
        else
        {
            Rabbit.SetActive(false);
            Cat.SetActive(false);
            Tortoise.SetActive(false);
        }

        Debug.Log($"Paquera: {paquera} // Pet: {pet} // Cena: {currentScene}");
    }
}
