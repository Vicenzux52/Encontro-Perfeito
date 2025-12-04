using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhonePanel : MonoBehaviour
{
    public string[] msgProtagonist, msgPaquera;
    public Sprite[] photos;
    public TMP_Text msg, msgP;
    public Image photo;

    private bool active = false;

    public void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void SetupPanel(int messageIndex, int paqueraIndex, int photoIndex)
    {
        if (messageIndex >= 0 && messageIndex < msgProtagonist.Length)
        {
            msg.text = msgProtagonist[messageIndex];
        }
            

        if (paqueraIndex >= 0 && paqueraIndex < msgPaquera.Length)
        {
            msgP.text = msgPaquera[paqueraIndex];
        }
           

        if (photoIndex >= 0 && photoIndex < photos.Length)
        {
            photo.sprite = photos[photoIndex];
        }
            
        active = true;
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}