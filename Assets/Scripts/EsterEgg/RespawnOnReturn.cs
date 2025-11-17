using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnOnReturn : MonoBehaviour
{
    void Start()
    {
        if (DeathSaver.hasSavedPosition && DeathSaver.estereggConcluido)
        {
            transform.position = DeathSaver.lastDeathPosition;
            DeathSaver.estereggConcluido = false;
        }
        else if (DeathSaver.emEasterEgg)
        {
            DeathSaver.emEasterEgg = false;
        }
    }
}
