using UnityEngine;

public class Portal : MonoBehaviour
{
    public AudioSource gameOverAudio;
    private HeartsUI heartsUI;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeathSaver.estereggConcluido = true;
            gameOverAudio.Play();
            PhotoAlbumManager.isGameOverEsterEgg = true;
            heartsUI.Win();
        }
    }
}
