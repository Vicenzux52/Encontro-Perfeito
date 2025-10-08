using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public GameObject textUI;
    public GameObject playPanel;
    public GameObject calendarPanel;
    public GameObject Door;
    public GameObject PhotoAlbum;
    public GameObject Radio;
    public GameObject Calendar;
    public GameObject wardrobePanel;
    public GameObject Wardrobe;

    public float timeCounter = 10f;
    private float time = 0f;

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                textUI.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == Door)
                {
                    playPanel.SetActive(true);
                }

                if (hit.collider.gameObject == PhotoAlbum || hit.collider.gameObject == Radio)
                {
                    textUI.SetActive(true);
                    time = timeCounter;
                }

                if (hit.collider.gameObject == Calendar)
                {
                    calendarPanel.SetActive(true);
                }

                if (hit.collider.gameObject == Wardrobe)
                {
                    wardrobePanel.SetActive(true);
                }
            }
        }
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
        wardrobePanel.SetActive(false);
    }
}
