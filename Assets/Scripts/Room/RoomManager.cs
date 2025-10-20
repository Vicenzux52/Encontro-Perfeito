using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    [Header("Ui Panels")]
    public GameObject textUI;
    public GameObject playPanel;
    public GameObject calendarPanel;
    public GameObject albumPanel;
    public GameObject pausePanel;

    [Header("Ui Timer")]
    public float timeCounter = 10f;
    private float time = 0f;

    [Header("Chibi's Opening Speech")]
    public GameObject dialoguePanel;
    public float duration = 3f;
    public float delayBeforeShow = 0.5f;
    private float timer;
    private bool isShowing;

    [Header("Interactable Objects")]
    public GameObject Door;
    public GameObject PhotoAlbum;
    public GameObject Radio;
    public GameObject Calendar;
    public GameObject Wardrobe;

    [Header("Chibi Reference")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public LayerMask collisionLayer;

    private int faseSelecionada = 0;

    private Rigidbody playerRb;
    private Vector3 targetPosition;
    private bool moving = false;
    private GameObject targetObject;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;

        if (dialoguePanel != null)
        {
            dialoguePanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("dialoguePanel não atribuido");
        }

        Invoke(nameof(ShowDialogue), delayBeforeShow);
    }

    void ShowDialogue()
    {
        if (dialoguePanel == null)
        {
            return;
        }

        dialoguePanel.SetActive(true);
        isShowing = true;
        timer = duration;
    }

    void Update()
    {
        if (isShowing)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                dialoguePanel.gameObject.SetActive(false);
                isShowing = false;
            }
        }

        HandleTimer();
        HandleClick();
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    void HandleTimer()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
                textUI.SetActive(false);
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                moving = true;
                targetObject = hit.collider.gameObject;
            }
        }
    }

    void MovePlayer()
    {
        if (!moving)
        {
            return;
        }

        Vector3 direction = targetPosition - playerRb.position;
        direction.y = 0;
        float distance = direction.magnitude;

        if (distance > 0.1f)
        {
            Vector3 move = direction.normalized * moveSpeed * Time.fixedDeltaTime;
            playerRb.MovePosition(playerRb.position + move);
        }
        else
        {
            moving = false;
            if (targetObject != null)
                OnReachTarget();
        }
    }

    void RotatePlayer()
    {
        if (!moving) return;

        Vector3 direction = targetPosition - playerRb.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void OnReachTarget()
    {
        if (targetObject == null)
        {
            return;
        }

        if (targetObject == Door)
        {
            playPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (targetObject == Radio)
        {
            textUI.SetActive(true);
            time = timeCounter;
        }
        else if (targetObject == Calendar)
        {
            calendarPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (targetObject == PhotoAlbum)
        {
            albumPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (targetObject == Wardrobe)
        {
            SceneManager.LoadScene("RoomWardrobe");
        }

        targetObject = null;
    }

    public void SelecionarFase(int indiceFase)
    {
        if (FaseManager.Instance != null && FaseManager.Instance.FaseLiberada(indiceFase))
        {
            faseSelecionada = indiceFase;
            Debug.Log($"Fase {indiceFase + 1} selecionada!");
        }
        else
        {
            Debug.LogWarning($"Fase {indiceFase + 1} não está liberada!");
        }
    }

    public void StartButton()
    {
        if (FaseManager.Instance != null && FaseManager.Instance.FaseLiberada(faseSelecionada))
        {
            string nomeCena = ObterNomeCenaPorFase(faseSelecionada);
            SceneManager.LoadScene(nomeCena);
            Time.timeScale = 1f;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void PauseButton(GameObject pauseUI)
    {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    private string ObterNomeCenaPorFase(int indiceFase)
    {
        switch (indiceFase)
        {
            case 0: return "Fase1";
            case 1: return "Fase2";
            case 2: return "Fase3";
            case 3: return "Fase4";
            default: return "Fase1";
        }
    }

    public void BackButton()
    {
        playPanel.SetActive(false);
        calendarPanel.SetActive(false);
        albumPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackButtonWardrobe()
    {
        SceneManager.LoadScene("Room");
    }
}