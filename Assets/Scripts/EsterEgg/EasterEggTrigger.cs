using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEggTrigger : MonoBehaviour
{
    //[Header("Probabilidade (0 a 1)")]
    //[Range(0f, 1f)]
    //public float esterEggChance = 0.1f;

    private bool triggered = false;
    private UIController uIController;

    void Start()
    {
        uIController = FindObjectOfType<UIController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (triggered)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            triggered = true;
            float r = Random.value;

            //if (r <= esterEggChance)
            //{

            Vector3 pos = collision.transform.position;
            pos.x = 0f;

            DeathSaver.lastDeathPosition = pos;
            DeathSaver.hasSavedPosition = true;
            DeathSaver.returnScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("EsterEgg");

            //}
            /*else
            {
                PhotoAlbumManager.isGameOverDeath = true;
                
                if (uIController != null)
                {
                    uIController.GameOverDeath();
                }
                else
                {
                    Debug.LogError("UIController não encontrado!");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }*/
        }
    }
}