using UnityEngine;
using UnityEngine.SceneManagement;

public class EscolhaPaqueras : MonoBehaviour
{
    [Header("Interactable Objects")]
    public GameObject PaqueraF;
    public GameObject PaqueraM;
    public GameObject Rabbit;
    public GameObject Cat;
    public GameObject Tortoise;

    [Header("Text UI")]
    public GameObject Text;
    public GameObject TextPet;

    [Header("Camera")]
    public Camera mainCamera;

    private bool escolheuPaquera = false;
    private bool escolheuPet = false;

    void Start()
    {        
        Rabbit.SetActive(false);
        Cat.SetActive(false);
        Tortoise.SetActive(false);

        TextPet.SetActive(false);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject objectSelect = hit.collider.gameObject;

                if (!escolheuPaquera)
                {
                    if (objectSelect == PaqueraF)
                    {
                        SelectPaquera("Feminino");
                    }
                    else if (objectSelect == PaqueraM)
                    {
                        SelectPaquera("Masculino");
                    }
                }
                else if (!escolheuPet)
                {
                    if (objectSelect == Rabbit)
                    {
                        SelectPet("Coelho");
                    }
                    else if (objectSelect == Cat)
                    {
                        SelectPet("Gato");
                    }
                    else if (objectSelect == Tortoise)
                    {
                        SelectPet("Tartaruga");
                    }
                }
            }
        }
    }

    public void SelectPaquera(string paqueraGender)
    {
        PlayerPrefs.SetString("paqueraSelect", paqueraGender);
        PlayerPrefs.Save();

        PaqueraF.SetActive(false);
        PaqueraM.SetActive(false);
        Rabbit.SetActive(true);
        Cat.SetActive(true);
        Tortoise.SetActive(true);

        TextPet.SetActive(true);
        Text.SetActive(false);

        escolheuPaquera = true;
        Debug.Log("Paquera escolhido: " + paqueraGender);
    }

    public void SelectPet(string pet)
    {
        PlayerPrefs.SetString("petSelect", pet);
        PlayerPrefs.Save();

        escolheuPet = true;
        Debug.Log("Pet escolhido: " + pet);
        PlayerPrefs.DeleteKey("Upgrade");
        SceneManager.LoadScene("Room");
    }
}
