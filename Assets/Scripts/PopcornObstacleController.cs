using UnityEngine;

public class PopcornObstacleController : MonoBehaviour
{
    public Transform popcornPrefab;

    public float timeDestruction, spawX, timer, time;

    void Start()
    {
        //player = transform.Find("player");
    }

    void Update()
    {
        timer += Time.deltaTime;
        PopcornSpawn();
    }

    public void PopcornSpawn()
    {
        if (timer >= time)
        {
            timer = 0f;

            float randomX = Random.Range(-spawX, spawX);
            Vector3 spawnPosition = transform.position + new Vector3(0, 0, randomX);

            Transform popcorn = Instantiate(popcornPrefab, spawnPosition, Quaternion.identity);

            SetupParabolic(popcorn, popcorn.right);

            Destroy(popcorn.gameObject, timeDestruction);
        }
    }

    public void SetupParabolic(Transform popcorn, Vector3 direction)
    {
        float popcornSpeed = 11f;
        float popcornAngle = 45f;

        float angleRad = popcornAngle * Mathf.Deg2Rad;

        Vector3 horizontalDir = new Vector3(direction.x, 0f, direction.z).normalized;

        Rigidbody rb = popcorn.GetComponent<Rigidbody>();

        Vector3 force = horizontalDir * popcornSpeed * Mathf.Cos(angleRad);
        force.y = popcornSpeed * Mathf.Sin(angleRad);

        rb.AddForce(force, ForceMode.VelocityChange);
    }
}
