using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnOnReturn : MonoBehaviour
{
    void Start()
    {
        if (DeathSaver.hasSavedPosition)
        {
            transform.position = DeathSaver.lastDeathPosition;

            DeathSaver.hasSavedPosition = false;
        }
    }
}
