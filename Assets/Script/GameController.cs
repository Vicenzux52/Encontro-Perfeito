using UnityEngine;

public class GameController : MonoBehaviour
{
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FrontalMovement();
    }

    void FrontalMovement()
    {
        transform.position += new Vector3(0, 0, player.GetComponent<Player>().frontSpeed * Time.deltaTime);
    }
}
