using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEggTrigger : MonoBehaviour
{
    [Header("Configurações do Easter Egg")]
    [Range(0f, 1f)]
    public float esterEggChance = 0.1f;

    private bool triggered = false;

    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (triggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            float r = Random.value;

            if (r <= esterEggChance)
            {
                triggered = true;
                SavePlayerPosition(collision.transform.position);
                DeathSaver.emEasterEgg = true;
                SceneManager.LoadScene("EsterEgg");
            }
            else
            {
                PhotoAlbumManager.isGameOverDeath = true;
                
                UIController.GameOverDeath();
            }
        }
    }

    void SavePlayerPosition(Vector3 position)
    {
        DeathSaver.lastDeathPosition = position;
        DeathSaver.hasSavedPosition = true;
        DeathSaver.returnScene = SceneManager.GetActiveScene().name;
        DeathSaver.estereggConcluido = false;
        DeathSaver.estereggNaoConcluido = false;
    }
}