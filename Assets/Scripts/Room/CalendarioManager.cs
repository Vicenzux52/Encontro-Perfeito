/*using UnityEngine;

public class CalendarioManager : MonoBehaviour
{
    [Header("Ui Buttons")]
    public GameObject fasesButtons;
    public GameObject Fase1;
    public GameObject Fase2;
    public GameObject Fase3;
    public GameObject Fase4;

    [HideInInspector] public bool fase1;
    [HideInInspector] public bool fase2;
    [HideInInspector] public bool fase3;
    [HideInInspector] public bool fase4;

    void Start()
    {
        if (fasesButtons != null)
        {
            fasesButtons.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && fase1 == true)
        Fase1Button();
        Fase2Button();
        Fase3Button();
        Fase4Button();
    }

    void Fase1Button()
    {
        fase1 = true;
        fase2 = false;
        fase3 = false;
        fase4 = false;

        if (Input.GetMouseButtonDown(0))
        {
            Fase1.SetActive(true);
            Fase2.SetActive(false);
            Fase3.SetActive(false);
            Fase4.SetActive(false);
        }
    }
    void Fase2Button()
    {
        fase1 = false;
        fase2 = true;
        fase3 = false;
        fase4 = false;

        if (Input.GetMouseButtonDown(0))
        {
            Fase1.SetActive(false);
            Fase2.SetActive(true);
            Fase3.SetActive(false);
            Fase4.SetActive(false);
        }
    }
    void Fase3Button()
    {
        fase1 = false;
        fase2 = false;
        fase3 = true;
        fase4 = false;

        if (Input.GetMouseButtonDown(0))
        {
            Fase1.SetActive(false);
            Fase2.SetActive(false);
            Fase3.SetActive(true);
            Fase4.SetActive(false);
        }
    }
    void Fase4Button()
    {
        fase1 = false;
        fase2 = false;
        fase3 = false;
        fase4 = true;

        if (Input.GetMouseButtonDown(0))
        {
            Fase1.SetActive(false);
            Fase2.SetActive(false);
            Fase3.SetActive(false);
            Fase4.SetActive(true);
        }
    }
}
*/