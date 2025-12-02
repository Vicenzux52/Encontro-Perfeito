using UnityEngine;
using UnityEngine.SceneManagement;

public class EscolhaPaqueras : MonoBehaviour
{
    [Header("Interactable Objects")]
    public GameObject PaqueraF;
    public GameObject PaqueraM;

    [Header("Text UI")]
    public GameObject Text;

    [Header("Camera")]
    public Camera mainCamera;

    private bool escolheuPaquera = false;

    public AudioSource audioSource;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject objectSelect = hit.collider.gameObject;
                audioSource.Play();

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
            }
        }
    }

    public void SelectPaquera(string paqueraGender)
    {
        PlayerPrefs.SetString("paqueraSelect", paqueraGender);
        PlayerPrefs.Save();

        PaqueraF.SetActive(false);
        PaqueraM.SetActive(false);
        Text.SetActive(false);

        escolheuPaquera = true;
        SceneManager.LoadScene("Room");
        Debug.Log("Paquera escolhido: " + paqueraGender);
    }
}
