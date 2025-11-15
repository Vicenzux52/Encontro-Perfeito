using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEggTrigger : MonoBehaviour
{
    [Header("Probabilidade (0 a 1)")]
    [Range(0f, 1f)]
    public float esterEggChance = 1f;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLIDI COM: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            float r = Random.value;

            Debug.Log("R = " + r);

            if (r <= esterEggChance)
            {
                DeathSaver.lastDeathPosition = collision.transform.position;
                DeathSaver.hasSavedPosition = true;

                DeathSaver.returnScene = SceneManager.GetActiveScene().name;

                SceneManager.LoadScene("VideoPortal");
            }
        }
    }
}

