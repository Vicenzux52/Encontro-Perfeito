using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    void Awake()
    {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
    }

}
