using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Colisão detectada");

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                //Debug.Log("Som tocando");
            }
        }
    }
}
