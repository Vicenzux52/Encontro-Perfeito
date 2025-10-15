using UnityEngine;

public class WardrobeManager : MonoBehaviour
{
    [Header("Assets")]
    public GameObject ProtagonistBelt;
    public GameObject ProtagonistClock;
    public GameObject ProtagonistHairClip;
    public GameObject ProtagonistTamagotchi;

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
    }
    public void HairClipButton()
    {
        DeactivateAll();
        ProtagonistHairClip.SetActive(true);
    }
    public void ClockButton()
    {
        DeactivateAll();
        ProtagonistClock.SetActive(true);
    }
    public void BeltButton()
    {
        DeactivateAll();
        ProtagonistBelt.SetActive(true);
    }
}
