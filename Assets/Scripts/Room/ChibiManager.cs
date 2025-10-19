using UnityEngine;

public class ChibiManager : MonoBehaviour
{
    public GameObject ChibiBelt;
    public GameObject ChibiClock;
    public GameObject ChibiHairClip;
    public GameObject ChibiTamagotchi;

    void Start()
    {
        ChibiBelt.SetActive(false);
        ChibiClock.SetActive(false);
        ChibiHairClip.SetActive(false);
        ChibiTamagotchi.SetActive(false);

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
    }
}
