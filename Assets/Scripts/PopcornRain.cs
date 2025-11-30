using UnityEngine;

public class PopcornRain : MonoBehaviour
{
    public GameObject popcorn;
    public Transform player;
    public float spawnDelay = 0.5f;
    float spawnCounter = 0;
    public float DeadTime = 1.5f;

    // Update is called once per frame
    void Update()
    {
        spawnCounter += Time.deltaTime;
        if (player.position.z < 1370 && player.position.z > 1052 && spawnCounter > spawnDelay)
        {
            transform.position = 
            Vector3.right * transform.position.x + Vector3.up * transform.position.y + Vector3.forward * (player.position.z + 6);
            Destroy(Instantiate(popcorn, transform.position + Vector3.forward* Random.Range(-6, 6), Quaternion.identity), DeadTime);
            spawnCounter = 0;
        }
    }
}
