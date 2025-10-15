using UnityEngine;
using UnityEngine.SceneManagement;

public class EscolhaPaqueras : MonoBehaviour
{
    [Header("Interactable Objects")]
    public GameObject PaqueraF;
    public GameObject PaqueraM;

    [Header("Camera")]
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject == PaqueraF || hit.transform.gameObject == PaqueraM)
                {
                    SceneManager.LoadScene("Room");
                }
            }
        }
    }
}
