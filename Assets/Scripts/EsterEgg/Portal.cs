using UnityEngine;

public class Portal : MonoBehaviour
{
    private HeartsUI heartsUI;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeathSaver.estereggConcluido = true;
            PhotoAlbumManager.isGameOverEsterEgg = true;
            heartsUI.Win();
        }
    }
}
