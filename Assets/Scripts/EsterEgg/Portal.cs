using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeathSaver.estereggConcluido = true;
            PhotoAlbumManager.isGameOverEsterEgg = true;
            
            if (HeartsUI.Instance != null)
            {
                HeartsUI.Instance.Win();
            }
            else
            {
                Debug.LogError("HeartsUI.Instance não encontrado!");
                SceneManager.LoadScene(DeathSaver.returnScene);
            }
        }
    }
}