using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    [Header("Ui Panels")]
    public GameObject textUI;
    public GameObject playPanel;
    public GameObject calendarPanel;

    [Header("Ui Timer")]
    public float timeCounter = 10f;
    private float time = 0f;

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

    private Rigidbody playerRb;
    private Vector3 targetPosition;
    private bool moving = false;
    private GameObject targetObject;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
    }

    void Update()
    {
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
        if (!moving) return;

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
        }
        else if (targetObject == PhotoAlbum || targetObject == Radio)
        {
            textUI.SetActive(true);
            time = timeCounter;
        }
        else if (targetObject == Calendar)
        {
            calendarPanel.SetActive(true);
        }
        else if (targetObject == Wardrobe)
        {
            SceneManager.LoadScene("RoomWardrobe");
        }

        targetObject = null;
    }

    public void StartButton()
    {
        SceneManager.LoadScene(2);
    }

    public void BackButton()
    {
        playPanel.SetActive(false);
    }

    public void BackButtonCalendar()
    {
        calendarPanel.SetActive(false);
    }

    public void BackButtonWardrobe()
    {
        SceneManager.LoadScene("Room");
    }
}
