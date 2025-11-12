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

    //[Header("Ui Timer")]
    //public float timeCounter = 3f;
    //private float time = 0f;

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

    [Header("Fases")]
    private int faseSelecionada = 0;

    [HideInInspector] public WardrobeManager WardrobeManager;

    [Header("Paqueras")]
    public GameObject PaqueraMText;
    public GameObject PaqueraFText;
    private CarregarEscolhaPaqueras carregarEscolhaPaqueras;

    [Header("Mensagens de Fase")]
    public GameObject mensagemInicial;
    public GameObject mensagemFase1;
    public GameObject mensagemFase2;
    public GameObject mensagemFase3;

    public TutorialRoom tutorialRoom;

    IEnumerator Start()
    {
        TutorialRoom tutorialRoom = FindFirstObjectByType<TutorialRoom>();
        if (tutorialRoom != null)
            yield return new WaitUntil(() => tutorialRoom.finishTutorial);

        IniciarJogo();

        yield return new WaitForSecondsRealtime(delayBeforeShow);
        ShowPaqueraTextPanel();

        yield return new WaitForSecondsRealtime(duration);

        if (paqueraTextPanel != null)
            paqueraTextPanel.SetActive(false);

        ShowDialogue();
    }


    void IniciarJogo()
    {
        Debug.Log("[RoomManager] Tutorial finalizado, iniciando jogo...");
        Debug.Log("[RoomManager] Chamando Invoke para ShowPaqueraTextPanel...");
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
            Debug.LogWarning("CarregarEscolhaPaqueras não encontrada na cena");

        if (FaseManager.Instance != null)
        {
            Invoke(nameof(MostrarMensagemPorFase), 0.3f);
        }
        else
        {
            Debug.LogWarning("FaseManager não encontrado na cena");
        }

        if (PlayerPrefs.GetInt("AtualizarMensagens", 0) == 1)
        {
            MostrarMensagemPorFase();
            PlayerPrefs.SetInt("AtualizarMensagens", 0);
            PlayerPrefs.Save();
            Debug.Log("Mensagens atualizadas após completar a fase");
        }
    }

    void ShowPaqueraTextPanel()
    {
        Debug.Log("[RoomManager] Entrou em ShowPaqueraTextPanel()");

        if (paqueraTextPanel == null)
        {
            return;
        }

        paqueraTextPanel.SetActive(true);
        audioSource.PlayOneShot(mensageNotification);
        isShowingText = true;
        textTimer = duration;

        if (PaqueraFText == null || PaqueraMText == null)
        {
            Debug.LogWarning("PaqueraFText ou PaqueraMText não atribuídos");
        }

        string paqueraSelecionada = (carregarEscolhaPaqueras != null)
            ? carregarEscolhaPaqueras.paquera
            : PlayerPrefs.GetString("paqueraSelect", "Feminino");

        PaqueraFText.SetActive(paqueraSelecionada == "Feminino");
        PaqueraMText.SetActive(paqueraSelecionada == "Masculino");

        MostrarMensagemPorFase();
    }

    private void CompletarFaseAtual()
    {
        if (faseSelecionada >= 0 && faseSelecionada < 3)
        {
            PlayerPrefs.SetInt($"Fase{faseSelecionada}", 1);
            PlayerPrefs.Save();

            Debug.Log($"Fase {faseSelecionada + 1} completada!");

            PlayerPrefs.SetInt("AtualizarMensagens", 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("Índice de fase inválido!");
        }
    }

    public void MostrarMensagemPorFase()
    {
        if (FaseManager.Instance == null)
        {
            Debug.LogWarning("FaseManager não encontrado");
            return;
        }

        mensagemInicial.SetActive(false);
        mensagemFase1.SetActive(false);
        mensagemFase2.SetActive(false);
        mensagemFase3.SetActive(false);

        bool fase1 = FaseManager.Instance.FaseCompletada(0);
        bool fase2 = FaseManager.Instance.FaseCompletada(1);
        bool fase3 = FaseManager.Instance.FaseCompletada(2);

        Debug.Log($"[DEBUG] Fase1={fase1}, Fase2={fase2}, Fase3={fase3}");

        if (fase3)
            mensagemFase3.SetActive(true);
        else if (fase2)
            mensagemFase2.SetActive(true);
        else if (fase1)
            mensagemFase1.SetActive(true);
        else
            mensagemInicial.SetActive(true);
    }

    void ShowDialogue()
    {
        if (dialoguePanel == null)
            return;

        dialoguePanel.SetActive(true);
        StartCoroutine(HideDialogueAfterDelay(duration));
    }

    private IEnumerator HideDialogueAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
            {
                Pause();
                Time.timeScale = 0;
            }
            else
            {
                BackButton();
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

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
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
        pausePanel.SetActive(false);
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