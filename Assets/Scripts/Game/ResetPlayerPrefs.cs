using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("ResetPlayerPrefs rodou!");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
