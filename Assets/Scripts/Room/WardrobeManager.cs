using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WardrobeManager : MonoBehaviour
{
    [Header("Assets")]
    public GameObject ProtagonistBelt;
    public GameObject ProtagonistClock;
    public GameObject ProtagonistHairClip;
    public GameObject ProtagonistTamagotchi;

    [HideInInspector] public bool tamagotchi;
    [HideInInspector] public bool clock;
    [HideInInspector] public bool hairClip;
    [HideInInspector] public bool belt;

    /*[Header("Upgrade Indisponivel")]
    public GameObject indisponivel;
    public float duration = 1f;
    private float timer = 0f;
    private bool isShowing = false;*/

    static public bool backToRoomWardrobe;

    /*void Update()
    {
        if (isShowing)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                indisponivel.SetActive(false);
                isShowing = false;
            }
        }
    }*/

    private void DeactivateAll()
    {
        ProtagonistBelt.SetActive(false);
        ProtagonistClock.SetActive(false);
        ProtagonistHairClip.SetActive(false);
        ProtagonistTamagotchi.SetActive(false);
    }

    public void TamagotchiButton()
    {
        DeactivateAll();
        ProtagonistTamagotchi.SetActive(true);
        tamagotchi = true;
        clock = false;
        hairClip = false;
        belt = false;
    }
    public void HairClipButton()
    {
        DeactivateAll();
        ProtagonistHairClip.SetActive(true);
        tamagotchi = false;
        clock = false;
        hairClip = true;
        belt = false;

        /*indisponivel.SetActive(true);
        timer = duration;
        isShowing = true;

        DeactivateAll();
        ProtagonistHairClip.SetActive(true);
        tamagotchi = false;
        clock = false;
        hairClip = true;
        belt = false;*/
    }
    public void ClockButton()
    {
        DeactivateAll();
        ProtagonistClock.SetActive(true);
        tamagotchi = false;
        clock = true;
        hairClip = false;
        belt = false;
    }
    public void BeltButton()
    {
        DeactivateAll();
        ProtagonistBelt.SetActive(true);
        tamagotchi = false;
        clock = false;
        hairClip = false;
        belt = true;
    }

    public void BackButton()
    {
        if (tamagotchi) PlayerPrefs.SetInt("UpgradeID", 0);
        else if (clock) PlayerPrefs.SetInt("UpgradeID", 1);
        else if (hairClip) PlayerPrefs.SetInt("UpgradeID", 2);
        else if (belt) PlayerPrefs.SetInt("UpgradeID", 3);

        PlayerPrefs.Save();
        Debug.Log("Acessorio salvo (ID): " + PlayerPrefs.GetInt("UpgradeID"));

        SceneManager.LoadScene("Room");
        backToRoomWardrobe = true;
    }
}
