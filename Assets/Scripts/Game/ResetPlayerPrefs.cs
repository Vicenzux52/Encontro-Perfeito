using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    void Awake()
    {
        #if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        #endif
    }

}
