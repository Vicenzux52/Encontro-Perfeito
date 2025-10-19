using UnityEngine;

public class UpgradeAcessorio : MonoBehaviour
{
    public GameObject Tamagotchi;
    public GameObject Clock;
    public GameObject HairClip;
    public GameObject Belt;

    void Start()
    {
        Tamagotchi.SetActive(false);
        Clock.SetActive(false);
        HairClip.SetActive(false);
        Belt.SetActive(false);

        int id = PlayerPrefs.GetInt("UpgradeID", -1);
        Debug.Log($"[PlayerUpgrades] Aplicando upgrade ID: {id}");

        switch (id)
        {
            case 0: Tamagotchi.SetActive(true); break;
            case 1: Clock.SetActive(true); break;
            case 2: HairClip.SetActive(true); break;
            case 3: Belt.SetActive(true); break;
            default: Debug.Log("Nenhum upgrade equipado"); break;
        }
    }
}