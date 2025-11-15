using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeathSaver.estereggConcluido = true;
            SceneManager.LoadScene("VideoPortal");
        }
    }
}
