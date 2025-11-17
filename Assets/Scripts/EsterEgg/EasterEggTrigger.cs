using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEggTrigger : MonoBehaviour
{
    [Header("Probabilidade (0 a 1)")]
    [Range(0f, 1f)]
    public float esterEggChance = 1f;

    private bool triggered = false;

    void OnCollisionEnter(Collision collision)
    {
        if (triggered)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            float r = Random.value;

            if (r <= esterEggChance)
            {
                triggered = true;

                DeathSaver.lastDeathPosition = collision.transform.position;
                DeathSaver.hasSavedPosition = true;
                DeathSaver.returnScene = SceneManager.GetActiveScene().name;

                SceneManager.LoadScene("EsterEgg");
            }
        }
    }
}
