using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    [Header("Ui Panels")]
    //public GameObject textUI;
    public GameObject playPanel;
    public GameObject calendarPanel;
    public GameObject albumPanel;
    public GameObject pausePanel;
    public GameObject paqueraTextPanel;

    [Header("Ui Timer")]
    public float timeCounter = 10f;
    private float time = 0f;

    [Header("Chibi's Opening Speech")]
    public GameObject dialoguePanel;
    public float duration = 2f;
    public float delayBeforeShow = 0.5f;
    private float dialoguetimer;
    private float textTimer;
    private bool isShowing;
    private bool isShowingText;

    [Header("Audio Source")]
    public AudioSource audioSource;
    public AudioClip mensageNotification;

    [Header("Interactable Objects")]
    public GameObject Door;
    public GameObject PhotoAlbum;
    public GameObject Radio;
    public GameObject CalendarIcon;
    public GameObject Wardrobe;

    [Header("Chibi Reference")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public LayerMask collisionLayer;
    private Rigidbody playerRb;
    private Vector3 targetPosition;
    private bool moving = false;
    private GameObject targetObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool returning = false;

    [Header("Upgrades")]
    public GameObject ChibiBelt;
    public GameObject ChibiClock;
    public GameObject ChibiHairClip;
    public GameObject ChibiTamagotchi;

    private int faseSelecionada = 0;

    [HideInInspector] public WardrobeManager WardrobeManager;

    [Header("Paqueras")]
    private CarregarEscolhaPaqueras carregarEscolhaPaqueras;
    public GameObject PaqueraMText;
    public GameObject PaqueraFText;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        originalPosition = playerRb.position;
        originalRotation = playerRb.rotation;
        playerRb.freezeRotation = true;

        ChibiBelt.SetActive(false);
        ChibiClock.SetActive(false);
        ChibiHairClip.SetActive(false);
        ChibiTamagotchi.SetActive(false);

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        int id = PlayerPrefs.GetInt("UpgradeID", -1);
        Debug.Log($"[ChibiManager] Aplicando upgrade ID: {id}");

        switch (id)
        {
            case 0: ChibiTamagotchi.SetActive(true); break;
            case 1: ChibiClock.SetActive(true); break;
            case 2: ChibiHairClip.SetActive(true); break;
            case 3: ChibiBelt.SetActive(true); break;
            default: Debug.Log("Nenhum upgrade equipado na Chibi"); break;
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("dialoguePanel não atribuido");
        }

        if (paqueraTextPanel != null)
        {
            paqueraTextPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("paqueraTextPanel não atribuido");
        }


        carregarEscolhaPaqueras = FindFirstObjectByType<CarregarEscolhaPaqueras>();
        if (carregarEscolhaPaqueras == null)
        Debug.LogWarning("CarregarEscolhaPaqueras não encontrada na cena. Considere arrastar no Inspector.");

        Invoke(nameof(ShowPaqueraTextPanel), delayBeforeShow);
    }

    void ShowPaqueraTextPanel()
    {
        if (paqueraTextPanel == null)
        {
            return;
        }

        paqueraTextPanel.SetActive(true);
        audioSource.PlayOneShot(mensageNotification);
        isShowingText = true;
        textTimer = duration;

        if (paqueraTextPanel == null)
        {
            Debug.LogWarning("paqueraTextPanel não atribuído");
            return;
        }

        if (PaqueraFText == null || PaqueraMText == null)
        {
            Debug.LogWarning("PaqueraFText ou PaqueraMText não atribuídos");
        }

        string paqueraSelecionada = (carregarEscolhaPaqueras != null)
            ? carregarEscolhaPaqueras.paquera
            : PlayerPrefs.GetString("paqueraSelect", "Feminino");

        PaqueraFText.SetActive(paqueraSelecionada == "Feminino");
        PaqueraMText.SetActive(paqueraSelecionada == "Masculino");
    }

    void ShowDialogue()
    {
        if (dialoguePanel == null)
        {
            return;
        }

        dialoguePanel.SetActive(true);
        isShowing = true;
        dialoguetimer = duration;
    }

    void Update()
    {
        if (isShowing)
        {
            dialoguetimer -= Time.deltaTime;

            if (dialoguetimer <= 0f)
            {
                dialoguePanel.gameObject.SetActive(false);
                isShowing = false;
            }
        }

        if (isShowingText)
        {
            textTimer -= Time.deltaTime;

            if (textTimer <= 0f)
            {
                paqueraTextPanel.gameObject.SetActive(false);
                isShowingText = false;
                paqueraTextPanel.SetActive(false);
                Invoke(nameof(ShowDialogue), delayBeforeShow);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
            {
                Resume(pausePanel);
            }
            else
            {
                Pause(pausePanel, dialoguePanel);
            }
        }

        if (WardrobeManager.backToRoomWardrobe == true)
        {
            dialoguePanel.SetActive(false);
            paqueraTextPanel.SetActive(false);
        }

        //HandleTimer();
        HandleClick();
    }

    public static void Resume(GameObject pausePanel)
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public static void Pause(GameObject pausePanel, GameObject dialoguePanel)
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        dialoguePanel.SetActive(false);
    }

    public static void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    /*void HandleTimer()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
                textUI.SetActive(false);
        }
    }*/

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionLayer))
            {
                originalPosition = playerRb.position;
                originalRotation = playerRb.rotation;

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

            if (returning)
            {
                returning = false;
                StartCoroutine(RotateToOriginal());
            }
            else if (targetObject != null)
                OnReachTarget();
        }
    }

    IEnumerator RotateToOriginal()
    {
        Quaternion startRotation = player.rotation;
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            player.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.rotation = originalRotation;
    }

    void RotatePlayer()
    {
        if (!moving) return;
        if (returning) return;

        Quaternion targetRotation;

        if (targetObject != null)
        {
            Vector3 direction = targetPosition - playerRb.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                targetRotation = Quaternion.LookRotation(direction);
            }
            else
            {
                return; 
            }
        }
        else
        {
            targetRotation = originalRotation;
        }
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
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
            dialoguePanel.SetActive(false);
            Time.timeScale = 0f;
        }
        else if (targetObject == Radio)
        {
            dialoguePanel.SetActive(false);
            StartCoroutine(ReturnToRadioRoutine());
        }
        /*else if (targetObject == Calendar)
        {
            calendarPanel.SetActive(true);
            Time.timeScale = 0f;
        }*/
        else if (targetObject == PhotoAlbum)
        {
            albumPanel.SetActive(true);
            CalendarIcon.SetActive(false);
            dialoguePanel.SetActive(false);
            Time.timeScale = 0f;
        }
        else if (targetObject == Wardrobe)
        {
            SceneManager.LoadScene("RoomWardrobe");
        }

        targetObject = null;
    }
    
    IEnumerator ReturnToRadioRoutine()
    {
        moving = false;

        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(RotateToOriginal());
        yield return new WaitForSeconds(0.3f);

        returning = true;
        targetPosition = originalPosition;
        moving = true;
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

    public void CalendarButton()
    {
        Time.timeScale = 0f;
        calendarPanel.SetActive(true);
        CalendarIcon.SetActive(false);
    }

    public void BackButton()
    {
        playPanel.SetActive(false);
        calendarPanel.SetActive(false);
        albumPanel.SetActive(false);
        CalendarIcon.SetActive(true);
        Time.timeScale = 1f;

        targetPosition = originalPosition;
        moving = true;
        returning = true;
        targetObject = null;

        Vector3 direction = (targetPosition - player.position);
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            player.rotation = lookRotation;
        }
    }

    public void BackButtonWardrobe()
    {
        SceneManager.LoadScene("Room");
    }
}