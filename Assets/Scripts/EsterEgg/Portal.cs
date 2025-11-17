using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private HeartsUI heartsUI;

    void Start()
    {
        heartsUI = FindObjectOfType<HeartsUI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeathSaver.estereggConcluido = true;
            DeathSaver.emEasterEgg = false;
            PhotoAlbumManager.isGameOverEsterEgg = true;
            
            SceneManager.LoadScene(DeathSaver.returnScene);
        }
    }
}
